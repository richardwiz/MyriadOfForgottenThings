using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatts.NextGen.StatsData
{
    public class Fixtures
    {
        public virtual Int32 FixtureId { get; protected set; }
        public virtual String FixtureName { get; protected set; }
        public virtual Int32 GeneralUpdates { get; protected set; }
        public virtual Int32 GeneralUpdatesZMU { get; protected set; }
        public virtual Int32 GeneralAvg { get; protected set; }
        public virtual Int32 GeneralMin { get; protected set; }
        public virtual Int32 GeneralLQ { get; protected set; }
        public virtual Int32 GeneralMed { get; protected set; }
        public virtual Int32 GeneralUQ { get; protected set; }
        public virtual Int32 GeneralMax { get; protected set; }
        public virtual Int32 SnapshotUpdates { get; protected set; }
        public virtual Int32 SnapshotUpdatesZMU { get; protected set; }
        public virtual Int32 SnapshotAvg { get; protected set; }
        public virtual Int32 SnapshotMin { get; protected set; }
        public virtual Int32 SnapshotLQ { get; protected set; }
        public virtual Int32 SnapshotMed { get; protected set; }
        public virtual Int32 SnapshotUQ { get; protected set; }
        public virtual Int32 SnapshotMax { get; protected set; }

        public Fixtures() : this(0, String.Empty,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0){ }
        public Fixtures
            (        
            Int32 fixtureId,
            String fixtureName,
            Int32 generalUpdates,
            Int32 generalUpdatesZmu,
            Int32 generalAvg,
            Int32 generalMin,
            Int32 generalLQ,
            Int32 generalMed,
            Int32 generalUQ,
            Int32 generalMax,
            Int32 snapshotUpdates,
            Int32 snapshotUpdatesZmu,
            Int32 snapshotAvg,
            Int32 snapshotMin,
            Int32 snapshotLQ,
            Int32 snapshotMed,
            Int32 snapshotUQ,
            Int32 snapshotMax
            )
        {
            this.FixtureId = fixtureId;
            this.FixtureName = fixtureName;
            this.GeneralUpdates = generalUpdates;
            this.GeneralUpdatesZMU = generalUpdatesZmu;
            this.GeneralAvg = generalAvg;
            this.GeneralMin = generalMin;
            this.GeneralLQ = generalLQ;
            this.GeneralMed = generalMed;
            this.GeneralUQ = generalUQ;
            this.GeneralMax = generalMax;
            this.SnapshotUpdates = snapshotUpdates;
            this.SnapshotUpdatesZMU = snapshotUpdatesZmu;
            this.SnapshotAvg = snapshotAvg;
            this.SnapshotMin = snapshotMin;
            this.SnapshotLQ = snapshotLQ;
            this.SnapshotMed = snapshotMed;
            this.SnapshotUQ = snapshotUQ;
            this.SnapshotMax = snapshotMax;

        }

    }
}
