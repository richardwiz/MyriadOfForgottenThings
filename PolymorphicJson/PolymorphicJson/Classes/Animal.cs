using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicJson.Classes
{
    [DataContract]
    [KnownType(typeof(Mammal))]
    [KnownType(typeof(Bird))]
    public abstract class Animal
    {
        [DataMember]
        public int numberOfLimbs { get; set; }

        [DataMember]
        public double weight { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Number of limbs: {0}", numberOfLimbs);
            sb.AppendLine();
            sb.AppendFormat("Typical adult weight: {0} pounds", weight);
            sb.AppendLine();
            return sb.ToString();
        }

    }
}
