using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicJson.Classes
{
    [DataContract]

    public class Robin : Bird
    {
        [DataMember]
        public string gender { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat("Gender: {0}", gender );
            sb.AppendLine();
            return sb.ToString();

        }

    }
}
