using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedTxnLib;

namespace Cerberus
{
    public class TxLogInfo
    {
        public UInt64 firstSerialNo;
        public UInt64 lastSerialNo;
        public DateTime firstTxnTime;
        public DateTime lastTxnTime;

        public TxLogInfo()
        {
            Initialize();
        }

        public void Initialize()
        {
            firstSerialNo = 0;
            lastSerialNo = UInt64.MaxValue;
        }

        public void GetLogDetails(String logFile)
        {
            using (MTxnLogFile TxnLogFile = new MTxnLogFile())
            {
                if (!TxnLogFile.Open(logFile))
                {
                    throw new ApplicationException("Failed to open TxnLogFile " + logFile);
                }

                TxnHeader header = null;
                UInt64 recPos;

                recPos = 0;
                header = TxnLogFile.GetTxnHeader(ref recPos);
                firstSerialNo = header.SerialNo;
                firstTxnTime = header.TxnTime;

                recPos = TxnLogFile.GetLastPosition();
                header = TxnLogFile.GetTxnHeader(ref recPos);
                lastSerialNo = header.SerialNo;
                lastTxnTime = header.TxnTime;
            }
        }

    };

}
