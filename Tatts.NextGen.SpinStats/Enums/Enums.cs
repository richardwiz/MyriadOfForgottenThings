using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatts.NextGen.SpinStats.Enums
{
    public enum UpdateType
    {
          Unknown = 0
        , General = 1
        , Snapshot = 2
    }

    public enum UpdateState
    {
        Unknown = 0
        , Misaligned = 1
        , OK = 2
    }
    public enum LineType
    {
        None = 0,
        UpdateStart = 1,
        UpdateComplete = 2,
        SnapshotStart = 3,
        SnapshotComplete = 4,
        MarketThread = 5,
        MarketSummary = 6,
        ResultMessage = 7,
        NoResultsIndicator = 8,
        OfferMapping = 9,
        OfferSelectionChange = 10
    }
}
