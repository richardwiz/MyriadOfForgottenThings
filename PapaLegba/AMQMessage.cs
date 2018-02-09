using Amqp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaLegba
{
   [AmqpContract(Name = "PapaLegba.AMQMessage", Encoding = EncodingType.Map)]
   public class AMQMessage
   {
      [AmqpMember(Name = "Payload", Order = 0)]
      public Object Payload { get; set; }

      public AMQMessage()
      {
         this.Payload = new Object();
      }
   }
}
