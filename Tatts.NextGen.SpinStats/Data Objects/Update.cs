using System;
using Tatts.NextGen.SpinStats.Enums;

namespace Tatts.NextGen.SpinStats
{

    public class Update
    {
        public DateTime StartTime;
        public DateTime FinishTime;
        public UpdateState State;
        public UpdateType Type;
        public int ThreadId;
        public long FixtureId;
        public string FixtureName;
        public int ObservedMarketUpdates;
        public int DeclaredMarketUpdates;
        public double Duration;
        public bool NoResultsObserved;
        public int OfferMappings;
        public int OfferSelectionChanges;

        public Update(DateTime startTime, int threadId, UpdateType type, string fixtureName)
        {
            this.StartTime = startTime;
            this.FinishTime = DateTime.MinValue;
            this.State = UpdateState.Unknown;
            this.Type = type;
            this.ThreadId = threadId;
            this.FixtureId = long.MinValue;
            this.FixtureName = fixtureName;
            this.ObservedMarketUpdates = 0;
            this.DeclaredMarketUpdates = 0;
            this.Duration = 0;
            this.NoResultsObserved = false;
            this.OfferMappings = 0;
            this.OfferSelectionChanges = 0;
        }

        public void Finalise(DateTime finishTime)
        {
            this.FinishTime = finishTime;

            if(finishTime >= StartTime)
            {
                TimeSpan span = finishTime - StartTime;
                this.Duration = span.TotalMilliseconds;
            }
            else
            {
                this.State = UpdateState.Misaligned;
            }
        }

    }
}
