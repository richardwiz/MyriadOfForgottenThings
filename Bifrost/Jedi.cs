using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amqp;
using Amqp.Serialization;

namespace Bifrost
{
   [AmqpContract]
    public class Jedi
    {
      [AmqpMember]
      public Int32 Force { get; set; }
      [AmqpMember]
      public String Name { get; set; }

      public Jedi() : this (0, String.Empty) { }

      public Jedi(Int32 force, String name)
      {
         this.Force = force;
         this.Name = name;
      }

      public override string ToString()
      {
         return String.Format("Jedi Master {0} has a force power rating of {1}", Name, Force);
      }
   }
}
