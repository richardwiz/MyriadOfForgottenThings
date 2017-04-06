using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Tatts.NextGen.StatsData
{
    public class ValidSubEventMap : ClassMap<ValidSubEvent>
    {
        public ValidSubEventMap()
        {
            Id(x => x.SubEventId);
            Table("ValidSubEvent");
        }
    }
}
