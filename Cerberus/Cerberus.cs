using Cerberus.Library;
using FluentCerberus;
using FluentCerberus.Connectivity;
using ManagedTxnLib;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;

namespace Cerberus
{
    public partial class Cerberus : ServiceBase
    {
        List<Int64> _ids = new List<Int64>();
        List<EFTTerminalAudit> _newTerminals = new List<EFTTerminalAudit>();
        List<Int64> _missingTxns = new List<long>();
        private Timer _timer;
        static String _cerberusConnection;
        static String _eisaConnection;

        public Cerberus()
        {
            InitializeComponent();
            _eisaConnection = ConfigurationManager.ConnectionStrings["Eisa"].ToString();
            _cerberusConnection = ConfigurationManager.ConnectionStrings["Cerberus"].ToString();
        }

        #region Service Control Methods
        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            Thread.Sleep(180000);
            //OnStop();
        }

        protected override void OnStart(string[] args)
        {
            // log start 
            StartTimer();
        }

        protected override void OnStop()
        {
            // Log stop
            StopTimer();
        }

        #endregion

        #region Timer Control Methods
        private void StartTimer()
        {
            try
            {
                if (_timer == null)
                {
                    int intervalSecs = 60;
                    TimeSpan tsInterval = new TimeSpan(0, 0, intervalSecs);
                    _timer = new Timer(new TimerCallback(DoWork), null, tsInterval, tsInterval);

                }
            }
            catch (Exception ex)
            {
                // log error
            }
        }

        private void StopTimer()
        {
            try
            {
                if (_timer != null)
                {
                    // Log("Stopping Timer");
                    _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    _timer.Dispose();
                    _timer = null;
                }
            }
            catch (Exception ex)
            {
                // Log("Error - Stop Timer : " + ex.Message.ToString());
            }

        }

        #endregion

        private void DoWork(Object state)
        {
            // Setup Variables
            DateTime ScanStartTxnTime = DateTime.MinValue;
            DateTime ScanEndTxnTime = DateTime.MaxValue;

            // Get Logs from Archive folder
            String path = Properties.Settings.Default.Properties["TxnLogPath"].DefaultValue.ToString();

            // Load up List of Logs to Scan
            String logPattern = ConfigurationManager.AppSettings["LogPattern"].ToString();
            List<String> logs = Directory.GetFiles(path, logPattern).ToList();

            // Result Lists
            List<TxnDetail> eftLogonTxns = new List<TxnDetail>();
            // Load _ids for adding to db
            _ids = CerberusTools.FindSerialNos(_eisaConnection);

            using (MTxnLogFile TxnLog = new MTxnLogFile())
            {
                try
                {
                    foreach (String log in logs)
                    {
                        if (!TxnLog.Open(log))
                        {
                            throw new ApplicationException("Unable to open file '" + log + "'");
                        }

                        // Get Log Info
                        TxLogInfo logInfo = new TxLogInfo();
                        logInfo.GetLogDetails(log);
                        // Get First and Last Serial Nos
                        UInt64 lastPosition = TxnLog.GetLastPosition();
                        UInt64 currentPosition = TxnLog.GetPosition(logInfo.firstSerialNo);
                        TxnDetail detail = TxnLog.GetTxnDetail(currentPosition);

                        // Iterate and find txn details from Log
                        foreach (ulong serialNo in _ids)
                        {
                            // 1: Get Txn Details
                            try
                            {
                                if (serialNo < logInfo.firstSerialNo || serialNo > logInfo.lastSerialNo)
                                {
                                    _missingTxns.Add((Int64)serialNo);
                                }
                                else
                                {
                                    currentPosition = TxnLog.GetPosition(serialNo);
                                    detail = TxnLog.GetTxnDetail(currentPosition);
                                    // 2: Load the EFT Logons
                                    if (detail.TxnId == 299)
                                    {
                                        // 3: Get the txn details
                                        eftLogonTxns.Add(detail);
                                        // Log event
                                    }
                                    else
                                    {
                                        // Log an error and continue;
                                    }
                                }
                            }
                            catch (ApplicationException aex)
                            {
                                // Log error
                                // Close Current Log
                                TxnLog.Close();
                                // Bail (to next file but out for now  )
                                break;
                            }

                        } // foreach (ulong serialNo in _ids)
                        TxnLog.Close();
                    } // foreach (String log in logs)
                    Int64 addedTxns = AddEftTxnsToSQL(eftLogonTxns);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            // If _newTerminals has any new terminals then Email or something
            if (_newTerminals.Count > 0)
            {
                // Format email
                // Send Email
                bool sent = CerberusTools.EmailNewTerminalsInfo(_newTerminals
                    , ConfigurationManager.AppSettings["RecipientList"].ToString().Split(new char[] { ',' }).ToList()
                    , ConfigurationManager.AppSettings["MailHost"].ToString()
                    , ConfigurationManager.AppSettings["FromAddress"].ToString());
            }
        }

        private Int64 AddEftTxnsToSQL(List<TxnDetail> eftLogonTxns)
        { 
            Int64 addedTxns = 0;
            // 4: Parse and load the Logons (as they have the pinpad id)
            foreach (TxnDetail eftTxn in eftLogonTxns)
            {
                // 4: Parse the Format String of details
                String eftDetail = eftTxn.FmtStr;
                Regex rexMerchantId = new Regex(ConfigurationManager.AppSettings["MerchantIdRegex"].ToString());
                Regex rexTerminalId = new Regex(ConfigurationManager.AppSettings["TerminalIdRegex"].ToString());
                Regex rexPinPadId = new Regex(ConfigurationManager.AppSettings["PinPadIdRegex"].ToString());
                Regex rexSWVersion = new Regex(ConfigurationManager.AppSettings["SWVersionIdRegex"].ToString());
                Regex rexStationNo = new Regex(ConfigurationManager.AppSettings["StationNoRegex"].ToString());
                Regex rexOutletId = new Regex(ConfigurationManager.AppSettings["OutletIdRegex"].ToString());

                EFTTerminalAudit eftta = new EFTTerminalAudit();
                eftta.PinPadId = Convert.ToInt64(rexPinPadId.Match(eftDetail).ToString());
                eftta.FirstVerified = DateTime.Now;
                eftta.LastVerified = DateTime.Now;
                eftta.Make = "Make";
                eftta.MerchantId = Convert.ToInt32(rexMerchantId.Match(eftDetail).ToString());
                eftta.Model = "Model";
                eftta.OfficeNo = Convert.ToInt32(rexOutletId.Match(eftDetail).ToString());
                eftta.StationNo = Convert.ToInt32(rexStationNo.Match(eftDetail).ToString());
                eftta.SWVersion = rexSWVersion.Match(eftDetail).ToString();
                eftta.TerminalId = rexTerminalId.Match(eftDetail).ToString();

                // Check if it is known
                if (! CerberusTools.IsKnownTerminal(eftta.PinPadId, _cerberusConnection))
                {
                    // 5: ADD to the database
                    using (ISession session = FluentNHibernateHelper.OpenSession(_cerberusConnection))
                    {
                        using (var txn = session.BeginTransaction())
                        {
                            session.Save(eftta);
                            txn.Commit();
                        }
                    }
                    _newTerminals.Add(eftta);
                    addedTxns++;
                }
            }
            return addedTxns;
        }
    }
}
