using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ManagedTxnLib;
using NHibernate;
using FluentCerberus;
using FluentCerberus.Connectivity;
using NHibernate.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace Cerberus
{
    public partial class Cerberus : ServiceBase
    {
        List<Int64> _ids = new List<Int64>();
        List<EFTTerminalAudit> _newTerminals = new List<EFTTerminalAudit>();
        List<Int64> _missingTxns = new List<long>();
        Timer _timer;

        public Cerberus()
        {
            InitializeComponent();
            var autoEvent = new AutoResetEvent(false);
            _timer = new Timer(DoWork, autoEvent, 1000, 250);
        }

        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            DoWork();
         }

        protected override void OnStop()
        {
            // Log stuff
        }

        private void DoWork()
        {
            // Setup Variables
            DateTime ScanStartTxnTime = DateTime.MinValue;
            DateTime ScanEndTxnTime = DateTime.MaxValue;

            // Get Logs from Archive folder
            String path = @"D:\EisaDir\data\TxnLogBackup\ProdExamples";

            // Load up List of Logs to Scan
            List<String> logs = Directory.GetFiles(path, "EisLog*.DAT").ToList();

            // Result Lists
            List<TxnDetail> eftLogonTxns = new List<TxnDetail>();
            // Load _ids for adding to db
            _ids = FindSerialNos();

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
                bool sent = CerberusTools.EmailNewTerminalsInfo(_newTerminals);
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
                Regex rexMerchantId = new Regex("(?<=MercantId:)[0-9]+");
                Regex rexTerminalId = new Regex("(?<=TerminalId:)[0-9A-Z]+");
                Regex rexPinPadId = new Regex("(?<=PinpadID:)[0-9]+");
                Regex rexSWVersion = new Regex("(?<=SWVersion:)[0-9.]+");
                Regex rexStationNo = new Regex("(?<=StationNo:)[0-9]+");
                Regex rexOutletId = new Regex("(?<=OutletId:)[0-9]+");

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
                if (! CerberusTools.IsKnownTerminal(eftta.PinPadId))
                {
                    // 5: ADD to the database
                    using (ISession session = FluentNHibernateHelper.OpenCerberusSession())
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

        private List<Int64> FindSerialNos()
        {
            // Load _ids
            using (ISession session = FluentNHibernateHelper.OpenEisaSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    return session.Query<EFTTransactionInfo>().Select(x => x.SerialNo).ToList();
                }
            }
        }
    }
}
