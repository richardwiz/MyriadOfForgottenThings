using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicJson.Classes
{
    [DataContract]

    public class Eagle : Bird
    {
        [DataMember]
        public int age { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat("Age: {0}", age);
            sb.AppendLine();
            return sb.ToString();

        }

    }
}
