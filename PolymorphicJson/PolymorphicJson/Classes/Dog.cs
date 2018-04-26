using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicJson.Classes
{
    [DataContract]

    public class Dog : Mammal
    {
        [DataMember]
        public string breed { get; set; }

        [DataMember]
        public double furLength { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat("Breed: {0}", breed);
            sb.AppendLine();
            sb.AppendFormat("Fur Length: {0} inches", furLength);
            sb.AppendLine();
            return sb.ToString();

        }

    }
}
