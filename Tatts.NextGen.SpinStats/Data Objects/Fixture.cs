using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterLogAnalyser
{
    public class Fixture
    {
        public long FixtureId;
        public string FixtureName;
        public List<Update> Updates;

        public double GeneralUpdates;
        public double GeneralUpdatesZMU;
        public double GeneralUpdateTimeAvg;
        public double GeneralUpdateTimeMin;
        public double GeneralUpdateTimeLQ;
        public double GeneralUpdateTimeMed;
        public double GeneralUpdateTimeUQ;
        public double GeneralUpdateTimeMax;

        public double SnapshotUpdates;
        public double SnapshotUpdatesZMU;
        public double SnapshotUpdateTimeAvg;
        public double SnapshotUpdateTimeMin;
        public double SnapshotUpdateTimeLQ;
        public double SnapshotUpdateTimeMed;
        public double SnapshotUpdateTimeUQ;
        public double SnapshotUpdateTimeMax;

        public Fixture(long fixtureId)
        {
            this.FixtureId = fixtureId;
            this.FixtureName = string.Empty;
            this.Updates = new List<Update>();

            this.GeneralUpdates = double.MinValue;
            this.GeneralUpdateTimeAvg = double.MinValue;
            this.GeneralUpdateTimeMin = double.MinValue;
            this.GeneralUpdateTimeLQ = double.MinValue;
            this.GeneralUpdateTimeMed = double.MinValue;
            this.GeneralUpdateTimeUQ = double.MinValue;
            this.GeneralUpdateTimeMax = double.MinValue;

            this.SnapshotUpdates = double.MinValue;
            this.SnapshotUpdateTimeAvg = double.MinValue;
            this.SnapshotUpdateTimeMin = double.MinValue;
            this.SnapshotUpdateTimeLQ = double.MinValue;
            this.SnapshotUpdateTimeMed = double.MinValue;
            this.SnapshotUpdateTimeUQ = double.MinValue;
            this.SnapshotUpdateTimeMax = double.MinValue;
        }
    }
}