using System;

namespace FluentCerberus
{
    /// <summary>
    /// SELECT TOP (100) PERCENT dbo.Trans.SerialNo, dbo.Trans.TxnId, dbo.TxnDesc.TxnName, dbo.Trans.TxnTime
    ///FROM dbo.Trans INNER JOIN
    /// dbo.TxnDesc ON dbo.Trans.TxnId = dbo.TxnDesc.TxnId
    ///WHERE (dbo.Trans.TxnId = 298) OR
    /// (dbo.Trans.TxnId = 299)
    ///ORDER BY dbo.Trans.SerialNo
    /// </summary>
    public class EFTTransactionInfo
    {
        public virtual Int64 SerialNo { get; protected set; }
        public virtual Int64 TxnId { get; protected set; }
        public virtual String TxnName { get; protected set; }
        public virtual DateTime TxnTime { get; protected set; }

        public EFTTransactionInfo()
        {
            this.SerialNo = 0;
            this.TxnId = 0;
            this.TxnName = String.Empty;
            this.TxnTime = DateTime.MinValue;
        }
    }
}
