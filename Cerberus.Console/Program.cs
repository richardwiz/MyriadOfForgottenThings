using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using ManagedTxnLib;
using Cerberus.Library;
using System.Text.RegularExpressions;
using FluentCerberus.Connectivity;
using NHibernate;
using FluentCerberus;

namespace Cerberus.Console
{
    class Program
    {
        static List<Int64> _ids = new List<Int64>();
        static List<EFTTerminalAudit> _newTerminals = new List<EFTTerminalAudit>();
        static List<EFTTerminalAudit> _movedTerminals = new List<EFTTerminalAudit>();
        static List<Int64> _missingTxns = new List<long>();
        static String _cerberusConnection;
        static String _eisaConnection;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            _eisaConnection = ConfigurationManager.ConnectionStrings["Eisa"].ToString();
            _cerberusConnection = ConfigurationManager.ConnectionStrings["Cerberus"].ToString();
            _log.Debug("Application started");
            ScanLogs();
        }

        private static void ScanLogs()
        {
            // Clean Out New Terminals
            if (_newTerminals.Count > 0)
                _newTerminals.Clear();

            if (_movedTerminals.Count > 0)
                _movedTerminals.Clear();

            // Setup Variables
            DateTime ScanStartTxnTime = DateTime.MinValue;
            DateTime ScanEndTxnTime = DateTime.MaxValue;
            // Get Logs from Archive folder
            String path = ConfigurationManager.AppSettings["TxnLogPath"].ToString();

            // Load up List of Logs to Scan
            String logPattern = ConfigurationManager.AppSettings["LogPattern"].ToString();
            List<String> logs = Directory.GetFiles(path, logPattern).ToList();

            // Result Lists
            List<TxnDetail> eftLogonTxns = new List<TxnDetail>();
            // Load _ids for adding to db
            //_ids = CerberusTools.FindSerialNos(_eisaConnection);

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
        private static Int64 AddEftTxnsToSQL(List<TxnDetail> eftLogonTxns)
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

                // Check if it is known- Check against PinPadId ONLY
                List<EFTTerminalAudit> existingTerminals = CerberusTools.GetTerminalByPinPadId(eftta.PinPadId, _cerberusConnection);
                if (existingTerminals != null && existingTerminals.Count > 0)
                {
                    // Check if Eft Terminal has moved
                    foreach (var eftTerm in existingTerminals)
                    {
                        if (eftTerm.OfficeNo != eftta.OfficeNo)
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
                }
                else // The terminal is New
                {
                    _newTerminals.Add(eftta);
                    CerberusTools.AddEftTerminal(eftta, _cerberusConnection);
                    addedTxns++;
                }
            }
            return addedTxns;
        }
    }
}
