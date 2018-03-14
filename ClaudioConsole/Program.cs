using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amqp;
using Rabbit = RabbitMQ.Client;
using System.Threading;

namespace ClaudioConsole
{
   class Program
   {
      static void Main(string[] args)
      {
         /*Rabbit MQ*/
         var factory = new Rabbit.ConnectionFactory() { HostName = "localhost" };

         using (var connection = factory.CreateConnection())
         {
            using (var channel = connection.CreateModel())
            {
               Console.Read();
               channel.ExchangeDeclare("logs", "fanout", true,false, null);

               channel.QueueDeclare(queue: "task_queue3"
                                    , durable: true
                                    , exclusive: false
                                    , autoDelete: false
                                    , arguments: null);
               // var queueName = channel.QueueDeclare().QueueName; // for disposable queue
               /*then use queue bind in place of Declare*/
               /*
               channel.QueueBind(queue: queueName,
                  exchange: "logs",
                  routingKey: "");*/
               for (int i = 0; i <= 10; i++)
               {             
                  string message = GetMessage(args, i);
                  Thread.Sleep(1000);

                  var body = Encoding.UTF8.GetBytes(message);
                  var properties = channel.CreateBasicProperties();
                  properties.Persistent = true;
                  channel.BasicQos(0, 1, false);

                  channel.BasicPublish(exchange: "logs"
                                       , routingKey: "task_queue3"
                                       , mandatory: false
                                       , basicProperties: properties
                                       , body: body);
                  Console.WriteLine("Sent: {0}", message);
               }
               Console.ReadKey();
            }
         }


         /*AMQ*/
         //string broker = args.Length >= 1 ? args[0] : "amqp://localhost:5672";
         //string address = args.Length >= 2 ? args[1] : "MarioBros";

         //Address brokerAddr = new Address(broker);
         //Connection connection = new Connection(brokerAddr);
         //Session session = new Session(connection);

         //SenderLink sender = new SenderLink(session, "helloworld-sender", address);
         //ReceiverLink receiver = new ReceiverLink(session, "helloworld-receiver", address);

         //Console.WriteLine("Please enter Message: ");
         //Message helloOut, helloIn;
         //String msg;
         //while ((msg = Console.ReadLine()) != "q")
         //{
         //   switch (msg)
         //   {
         //      case ">":
         //         msg = Console.ReadLine();
         //         helloOut = new Message(msg);
         //         sender.Send(helloOut);
         //         Console.WriteLine(String.Format("Message: {0}; sent to {1}:{2}", helloOut.Body.ToString(), broker, address));
         //         break;
         //      case "<":
         //         helloIn = receiver.Receive();
         //         receiver.Accept(helloIn);
         //         Console.WriteLine(String.Format("Message: {0}; recieved from {1}:{2}", helloIn.Body.ToString(), broker, address));
         //         break;
         //      default:
         //         helloOut = new Message(msg);
         //         sender.Send(helloOut);
         //         Console.WriteLine(String.Format("Message: {0}; sent to {1}:{2}", helloOut.Body.ToString(), broker, address));
         //         break;
         //   }
         //}

         //receiver.Close();
         //sender.Close();
         //session.Close();
         //connection.Close();
      }

      private static string GetMessage(string[] args, int count)
      {
         string dots = string.Empty;

         for (int i= count; i<= count; i++)
         {
            dots += "."; 
         }

         return ((args.Length > 0) ? string.Join(" ", args) + count.ToString() + dots : "Hello World!");
      }
   }
}
