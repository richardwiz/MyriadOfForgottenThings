using Cerberus.Library;
using FluentCerberus;
using FluentCerberus.Connectivity;
using log4net;
using ManagedTxnLib;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;
using CerberusExtensions;


namespace Cerberus
{
    public partial class Cerberus : ServiceBase
    {
        private List<Int64> _ids = new List<Int64>();
        private List<EFTTerminalAudit> _newTerminals = new List<EFTTerminalAudit>();
        private List<EFTTerminalAudit> _movedTerminals = new List<EFTTerminalAudit>();
        private List<Int64> _missingTxns = new List<Int64>();
        private Timer _timer;
        private static String _cerberusConnection;
        private static String _eisaConnection;
        //Here is the once-per-class call to initialize the log object
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Regexes
        private static Regex _rexMerchantId;
        private static Regex _rexTerminalId;
        private static Regex _rexPinPadId;
        private static Regex _rexSWVersion;
        private static Regex _rexStationNo;
        private static Regex _rexOutletId;
        #endregion

        public Cerberus()
        {
            InitializeComponent();
            _eisaConnection = ConfigurationManager.ConnectionStrings["Eisa"].ToString();
            _cerberusConnection = ConfigurationManager.ConnectionStrings["Cerberus"].ToString();
            _rexMerchantId = new Regex(ConfigurationManager.AppSettings["MerchantIdRegex"].ToString());
            _rexTerminalId = new Regex(ConfigurationManager.AppSettings["TerminalIdRegex"].ToString());
            _rexPinPadId = new Regex(ConfigurationManager.AppSettings["PinPadIdRegex"].ToString());
            _rexSWVersion = new Regex(ConfigurationManager.AppSettings["SWVersionIdRegex"].ToString());
            _rexStationNo = new Regex(ConfigurationManager.AppSettings["StationNoRegex"].ToString());
            _rexOutletId = new Regex(ConfigurationManager.AppSettings["OutletIdRegex"].ToString());

    }

    #region Service Control Methods
    public void RunAsConsole(string[] args)
        {
            _log.Info(" *** Running Cerberus in Console Mode...Standby...*** ");
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            _log.Info(" *** Warm kitty, soft kitty, little ball of fur. Sleepy kitty, happy kitty, purr purr purr ...*** ");
            Thread.Sleep(180000);
            //OnStop();
        }

        protected override void OnStart(string[] args)
        {
            // Log start
            _log.Info(" *** Starting Cerberus *** "); 
            StartTimer();
        }

        protected override void OnStop()
        {
            // Log stop
            _log.Info(" *** Stopping Cerberus *** ");
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
                _log.ErrorFormat("Error - Start Timer : {0}", ex.Message.ToString());
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
                _log.ErrorFormat("Error - Stop Timer : {0}", ex.Message.ToString());
            }

        }

        #endregion

        private void DoWork(Object state)
        {
            _log.Info(" ==> TICK: Cerberus is working *** ");

            // Clean Out New Terminals
            if (_newTerminals.Count > 0)
                _newTerminals.Clear();

            if (_movedTerminals.Count > 0)
                _movedTerminals.Clear();

            // Setup Variables
            DateTime ScanStartTxnTime = DateTime.MinValue;
            DateTime ScanEndTxnTime = DateTime.MaxValue;
            List<FileInfo> logs = new List<FileInfo>();

            // Get Logs from Archive folder
            String path = ConfigurationManager.AppSettings["TxnLogPath"].ToString();//Properties.Settings.Default.Properties["TxnLogPath"].ToString();
            String workingDir = ConfigurationManager.AppSettings["WorkingDir"].ToString(); //Properties.Settings.Default.Properties["WorkingDir"].ToString();
            String doneDir = ConfigurationManager.AppSettings["DoneDir"].ToString(); //Properties.Settings.Default.Properties["DoneDir"].ToString();
            Boolean getYesterdaysTxnLog = Convert.ToBoolean(ConfigurationManager.AppSettings["GetYesterdaysTxnLog"].ToString());
            // Load up List of Logs to Scan
            String logPattern = ConfigurationManager.AppSettings["LogPattern"].ToString();
            DirectoryInfo txnDir = new DirectoryInfo(path);

            // TODO: Might need to Copy file to CerberusDir for processing
            if (getYesterdaysTxnLog)
            {
                logs = txnDir.GetFiles(logPattern, SearchOption.TopDirectoryOnly)
                                .Where(f => f.CreationTime.Date == DateTime.Now.AddDays(-1).Date)
                                .ToList();
                _log.Info(String.Format(" Getting yesterdays Txn log; Date: {0}", DateTime.Now.AddDays(-1).Date));
            }
            else // Get them all in the folder
            {
                logs = txnDir.GetFiles(logPattern, SearchOption.TopDirectoryOnly)
                                .ToList();
                _log.Info(String.Format(" Getting all Txn logs."));
            }

            // Result Lists
            List<TxnDetail> eftLogonTxns = new List<TxnDetail>();
            // Load _ids for adding to db
            _ids = CerberusTools.FindSerialNos(_eisaConnection);

            using (MTxnLogFile TxnLog = new MTxnLogFile())
            {
                try
                {
                    foreach (FileInfo log in logs)
                    {
                        _log.Info(String.Format(" ==> PROCESSING Txn log; {0}", log.FullName));
                        // Copy to working Dir
                        String workingPath = Path.Combine(workingDir, log.Name);
                        File.Copy(log.FullName, workingPath, true);
                        _log.Info(String.Format(" ==> COPYING From; {0} To; {1}", log.FullName, workingPath));

                        new FileIOPermission(FileIOPermissionAccess.Read, log.FullName).Demand(); // Breaks the copy lock????
                        new FileIOPermission(FileIOPermissionAccess.Read, workingPath).Demand(); // Breaks the copy lock????

                        if (!TxnLog.Open(workingPath))
                        {
                            throw new ApplicationException("Unable to open file '" + log + "'");
                        }
                        _log.Info(String.Format(" ==> Txn log OPEN; {0} ", workingPath));

                        // Get Log Info
                        TxLogInfo logInfo = new TxLogInfo();
                        logInfo.GetLogDetails(workingPath);
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
                                        _log.InfoFormat(" ==> FOUND New EFT Terminal; {0} ", detail.FmtStr);
                                    }
                                }
                            }
                            catch (ApplicationException aex)
                            {
                                // Log error
                                _log.ErrorFormat(" ==> EXCEPTION processing SerialNo; {0}, Position; {1}, Message: {2}.", serialNo, currentPosition, aex.Message);
                                // Close Current Log
                                TxnLog.Close();
                                // Bail (to next file but out for now  )
                                _log.ErrorFormat(" ==> CLOSING Log; {0} ", workingPath);
                                break;
                            }

                        } // foreach (ulong serialNo in _ids)
                        TxnLog.Close();

                        // Move to Done - delete old one if exists
                        String donePath = Path.Combine(doneDir, log.Name);
                        if (File.Exists(donePath))
                        {
                            File.Delete(donePath);
                        }
                        _log.Info(String.Format(" ==> MOVING From; {0} To; {1}",  workingPath, donePath));

                        File.Move(workingPath, donePath);

                    } // foreach (String log in logs)
                    Int64 addedTxns = AddEftTxnsToSQL(eftLogonTxns);
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat(" ==> EXCEPTION; {0}.", ex.Message);
                    throw ex;
                }
            }

            // If _newTerminals has any new terminals then Email or something
            if (_newTerminals.Count > 0 || _movedTerminals.Count > 0)
            {
                // Format email
                // Send Email
                bool sent = CerberusTools.EmailNewTerminalsInfo(_newTerminals
                    , _movedTerminals
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

                EFTTerminalAudit eftta = new EFTTerminalAudit();
                eftta.PinPadId = Convert.ToInt64(_rexPinPadId.Match(eftDetail).ToString());
                String make, model = String.Empty;
                CerberusTools.GetEFTMakeAndModel(eftta.PinPadId, out make, out model);
                eftta.Make = make;
                eftta.Model = model;
                eftta.LastVerified = DateTime.Now;
                eftta.MerchantId = Convert.ToInt32(_rexMerchantId.Match(eftDetail).ToString());
                eftta.OfficeNo = Convert.ToInt32(_rexOutletId.Match(eftDetail).ToString());
                eftta.StationNo = Convert.ToInt32(_rexStationNo.Match(eftDetail).ToString());
                eftta.SWVersion = _rexSWVersion.Match(eftDetail).ToString();
                eftta.TerminalId = _rexTerminalId.Match(eftDetail).ToString();

                // Check if it is known- Check against PinPadId ONLY
                // As it may have moved offices multiple times
                List<EFTTerminalAudit> existingTerminals = CerberusTools.GetTerminalByPinPadId(eftta.PinPadId, _cerberusConnection);
                if (existingTerminals != null && existingTerminals.Count > 0)
                {
                    // Check if Eft Terminal has moved  - i.e it is NOT in existingTerminals
                    Boolean hasMoved = CerberusTools.HasTerminalMoved(existingTerminals, eftta);
                    if (hasMoved)
                    {
                        _movedTerminals.Add(eftta);
                        eftta.Status = Convert.ToInt32(TerminalAuditStatus.Moved);
                        _log.Info(String.Format(" ==> ADDING *MOVED* Terminal[PPID] {0} to SQL for Office{1} and Seat {2}.", eftta.PinPadId, eftta.OfficeNo, eftta.StationNo));
                        CerberusTools.AddEftTerminal(eftta, _cerberusConnection);
                        addedTxns++;
                    }
                    else
                    {
                        // Log terminal exists ??
                        _log.Info(String.Format(" ==> Terminal[PPID] {0} EXISTS at Office{1} and Seat {2}.", eftta.PinPadId, eftta.OfficeNo, eftta.StationNo));
                    }

                }
                else // The terminal is New
                {
                    _newTerminals.Add(eftta);
                    eftta.Status = Convert.ToInt32(TerminalAuditStatus.New);
                    eftta.FirstVerified = DateTime.Now;
                    _log.Info(String.Format(" ==> ADDING *NEW* Terminal[PPID] {0} to SQL for Office{1} and Seat {2}.", eftta.PinPadId, eftta.OfficeNo, eftta.StationNo));
                    CerberusTools.AddEftTerminal(eftta, _cerberusConnection);
                    addedTxns++;
                }
            }
            return addedTxns;
        }
    }
}
