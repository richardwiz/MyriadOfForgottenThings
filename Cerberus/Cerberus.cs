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

namespace Cerberus
{
    public partial class Cerberus : ServiceBase
    {
        public Cerberus()
        {
            InitializeComponent();
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
            // Setup Variables
            DateTime ScanStartTxnTime = DateTime.MinValue;
            DateTime ScanEndTxnTime = DateTime.MaxValue;

            // Open Log
            TxLogInfo logInfo = new TxLogInfo();
            String path = @"D:\EisaDir\data\TxnLogBackup\ProdExamples\EisLog02112.DAT";

            // Get Txns
            //List<TxnIdInfo> txnIds = MTxnLogFile.GetTxnIdInfo().Where(t => t.TxnId == 298 || t.TxnId == 299).ToList();

            // Get Results
            List<TxnDetail> results = new List<TxnDetail>();

            using (MTxnLogFile TxnLog = new MTxnLogFile())
            {
                try
                {
                    if (!TxnLog.Open(path))
                    {
                        throw new ApplicationException("Unable to open file '" + path + "'");
                    }

                    // Get Log Info
                    logInfo.GetLogDetails(path);
                    // Get First and Last Serial Nos
                    UInt64 lastPosition = TxnLog.GetLastPosition();
                    UInt64 currentPosition = TxnLog.GetPosition(logInfo.firstSerialNo);
                    // 1: Get Start Txn Details
                    //TxnHeader header = TxnLog.GetTxnHeader(ref currentPosition);
                    TxnDetail detail = TxnLog.GetTxnDetail(currentPosition);

                    while (currentPosition <= lastPosition)
                    {
                        // A: Housekeeping
                        //if (header.FailedFlag)
                        //{
                        //    // Log something
                        //    break;
                        //}

                        //if (header.EndOfScan)
                        //{
                        //    // Log header.TextMsg;
                        //    break;
                        //}

                        // 2: Check the Detail txnId to filter scan
                        if(detail.TxnId == 298 || detail.TxnId == 299)
                        {
                            // 3: Get the txn details
                            results.Add(TxnLog.GetTxnDetail(currentPosition));
                            // Log event
                        }

                        // 4: Move to next position
                        currentPosition = TxnLog.GetPosition(detail.SerialNo + 1);

                        // 5: Update the Detail
                        detail = TxnLog.GetTxnDetail(currentPosition);
                    }

                    // Parse the Format String of details
                    foreach (TxnDetail eftTxn in results)
                    {
                        String x = eftTxn.FmtStr;
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }

         }

        protected override void OnStop()
        {
        }
    }
}
