using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicJson.Classes
{
    [DataContract]
    [KnownType(typeof(Dog))]
    [KnownType(typeof(Cat))]

    public abstract class Mammal : Animal
    {
        [DataMember]
        public bool landBased { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat("Land Based Mammal: {0}", landBased );
            sb.AppendLine();
            return sb.ToString();

        }

    }
}
