using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Amqp;
using Amqp.Serialization;
using PapaLegba;
using Rabbit = RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace MarioConsole
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
               channel.ExchangeDeclare("logs", "fanout", true, false, null);
               channel.QueueDeclare(queue: "task_queue3"
                                    , durable: true
                                    , exclusive: false
                                    , autoDelete: false
                                    , arguments: null);
               /* Receiving from disposable queue*//*
             var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: "logs",
                              routingKey: "");*/

            Console.WriteLine(" [*] Waiting for logs.");
               var consumer = new EventingBasicConsumer(channel);

               consumer.Received += (model, ea) =>
               {
                  var body = ea.Body;
                  var message = Encoding.UTF8.GetString(body);
                  Console.WriteLine("Recieved: {0}", message);

                 // int dots = message.Split('.').Length - 1;
                 //Thread.Sleep(dots * 1000);

                 // Console.WriteLine("Done");
                 // channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

               };

               channel.BasicConsume(queue: "task_queue3"
                                    , autoAck: true
                                    , consumerTag: ""
                                    , noLocal: false
                                    , exclusive: false
                                    , arguments: null
                                    , consumer: consumer);
               Console.ReadLine();
            }
         }

         /*AMQ*/
         //string broker = args.Length >= 1 ? args[0] : "amqp://localhost:5672"; //Amq 
         //string address = args.Length >= 2 ? args[1] : "MarioBros"; // topic
         //Trace.TraceLevel = TraceLevel.Frame | TraceLevel.Verbose;

         //Address brokerAddr = new Address(broker);
         //Connection connection = new Connection(brokerAddr);

         //if (connection != null /*&& not closed ??*/)
         //{
         //   Session session = new Session(connection);

         //   SenderLink sender = new SenderLink(session, "helloworld-sender", address);
         //   ReceiverLink receiver = new ReceiverLink(session, "helloworld-receiver", address);

         //   Console.WriteLine("Please enter Message: ");
         //   Message helloOut, helloIn;
         //   String msg;
         //   String json = new JavaScriptSerializer().Serialize(new Jedi(97, "Razzrion"));
         //   Console.WriteLine(json);

         //   try
         //   {
         //      while ((msg = Console.ReadLine()) != "q")
         //      {
         //         switch (msg)
         //         {
         //            case ">":
         //               msg = Console.ReadLine();
         //               helloOut = new Message(json);
         //               sender.Send(helloOut);
         //               Console.WriteLine(String.Format("Message: {0}; sent to {1}:{2}", helloOut.Body.ToString(), broker, address));
         //               break;
         //            case "<":
         //               helloIn = receiver.Receive();
         //               receiver.Accept(helloIn);
         //               Console.WriteLine(String.Format("Message: {0}; recieved from {1}:{2}", helloIn.Body.ToString(), broker, address));
         //               break;
         //            default:
         //               helloOut = new Message();
         //               sender.Send(helloOut);
         //               Console.WriteLine(String.Format("Message: {0}; sent to {1}:{2}", helloOut.Body.ToString(), broker, address));
         //               break;
         //         }
         //      }
         //   }
         //   catch (AmqpException amqex)
         //   {
         //      Console.WriteLine(amqex.Message);
         //   }
         //   catch (ArgumentException argex)
         //   {
         //      Console.WriteLine(argex.Message);
         //   }
         //   catch (TimeoutException timex)
         //   {
         //      Console.WriteLine(timex.Message);
         //   }
         //   catch (Exception ex)
         //   {
         //      Console.WriteLine(ex.Message);
         //   }

         //   receiver.Close();
         //   sender.Close();
         //   session.Close();
         //   connection.Close();
      }
   }
}
