using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicJson.Classes
{
    [DataContract]
    [KnownType(typeof(Robin))]
    [KnownType(typeof(Eagle))]

    public abstract class Bird : Animal
    {
        [DataMember]
        public int eggIncubationTime { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendFormat("Egg Incubation Time: {0} weeks", eggIncubationTime);
            sb.AppendLine();
            return sb.ToString();

        }

    }
}
