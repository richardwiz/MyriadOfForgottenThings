using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Tatts.NextGen.StatsData
{
    public class FixturesMap : ClassMap<Fixtures>
    {
        public FixturesMap()
        {
            Id(x => x.FixtureId);
            Map(x => x.FixtureName);
            Map(x => x.GeneralUpdates);
            Map(x => x.GeneralAvg);
            Map(x => x.GeneralMin);
            Map(x => x.GeneralLQ);
            Map(x => x.GeneralMed);
            Map(x => x.GeneralUQ);
            Map(x => x.GeneralMax);
            Map(x => x.SnapshotUpdates);
            Map(x => x.SnapshotAvg);
            Map(x => x.SnapshotMin);
            Map(x => x.SnapshotLQ);
            Map(x => x.SnapshotMed);
            Map(x => x.SnapshotUQ);
            Map(x => x.SnapshotMax);
            Table("Fixtures");
        }
    }
}
