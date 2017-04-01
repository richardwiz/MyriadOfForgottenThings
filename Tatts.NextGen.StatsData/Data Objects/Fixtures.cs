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
        public virtual Int32 GeneralAvg { get; protected set; }
        public virtual Int32 GeneralMin { get; protected set; }
        public virtual Int32 GeneralLQ { get; protected set; }
        public virtual Int32 GeneralMed { get; protected set; }
        public virtual Int32 GeneralUQ { get; protected set; }
        public virtual Int32 GeneralMax { get; protected set; }
        public virtual Int32 SnapshotUpdates { get; protected set; }
        public virtual Int32 SnapshotAvg { get; protected set; }
        public virtual Int32 SnapshotMin { get; protected set; }
        public virtual Int32 SnapshotLQ { get; protected set; }
        public virtual Int32 SnapshotMed { get; protected set; }
        public virtual Int32 SnapshotUQ { get; protected set; }
        public virtual Int32 SnapshotMax { get; protected set; }

        public Fixtures()
        {
            this.FixtureId = 0;
            this.FixtureName = String.Empty; 
            this.GeneralUpdates = 0;
            this.GeneralAvg = 0;
            this.GeneralMin = 0;
            this.GeneralLQ = 0;
            this.GeneralMed = 0;
            this.GeneralUQ = 0;
            this.GeneralMax = 0;
            this.SnapshotUpdates = 0;
            this.SnapshotAvg = 0;
            this.SnapshotMin = 0;
            this.SnapshotLQ = 0;
            this.SnapshotMed = 0;
            this.SnapshotUQ = 0;
            this.SnapshotMax = 0;

        }
    }
}
