using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tatts.NextGen.StatsData
{
    public class ValidSubEvent
    {
        public virtual Int64 SubEventId { get; protected set; }

        public ValidSubEvent(Int64 subEventId)
        {
            this.SubEventId = subEventId;
        }
        public ValidSubEvent() : this(0) { }

    }
}
