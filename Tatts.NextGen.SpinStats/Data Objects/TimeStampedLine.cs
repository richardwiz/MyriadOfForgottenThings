using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterLogAnalyser
{
    public class TimeStampedLine
    {
        public DateTime TimeStamp;
        public string Value;

        public TimeStampedLine(DateTime timeStamp, string value)
        {
            this.TimeStamp = timeStamp;
            this.Value = value;
        }
    }
}
