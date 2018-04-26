using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicJson.Classes
{
    [DataContract]

    public class Cat : Mammal
    {
        [DataMember]
        public string eyeColor { get; set; }

        [DataMember]
        public bool hypoAllergenic { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat("Eye Color: {0}", eyeColor);
            sb.AppendLine();
            sb.AppendFormat("Hypoallergenic: {0} ", hypoAllergenic);
            sb.AppendLine();
            return sb.ToString();

        }

    }
}
