using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatts.NextGen.StatsData
{
    public class TxnTimeIndex
    {
        public virtual DateTime TxnTime { get; protected set; }
        public virtual Int64 SerialNo { get; protected set; }

        public TxnTimeIndex()
        {
            this.TxnTime = DateTime.MinValue;
            this.SerialNo = 0;
        }
    }
}
