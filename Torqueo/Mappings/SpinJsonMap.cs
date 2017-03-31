using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Torqueo.Mappings
{
    public class SpinJsonMap : ClassMap<SpinJson>
    {
        public SpinJsonMap()
        {
            Id(x => x.JsonId);
            Map(x => x.Sport);
            Map(x => x.Imported);
            Table("SpinJson");
        }
    }
}
