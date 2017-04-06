using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Tatts.NextGen.StatsData
{
    public class TxnTimeIndexMap : ClassMap<TxnTimeIndex>
    {
        public TxnTimeIndexMap()
        {
            Id(x => x.TxnTime).GeneratedBy.Assigned();
            Map(x => x.SerialNo);
            Table("TxnTimeIndex");
        }
    }
}
