﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amqp;
using Bifrost;

namespace LuigiConsole
{
   class Program
   {
      static void Main(string[] args)
      {
         string broker = args.Length >= 1 ? args[0] : "amqp://localhost:5672";
         string address = args.Length >= 2 ? args[1] : "MarioBros";
         Trace.TraceLevel = TraceLevel.Frame | TraceLevel.Verbose;

         Address brokerAddr = new Address(broker);
         Connection connection = new Connection(brokerAddr);

         if (connection != null /*&& not closed ??*/)
         {
            Session session = new Session(connection);

            SenderLink sender = new SenderLink(session, "helloworld-sender", address);
            ReceiverLink receiver = new ReceiverLink(session, "helloworld-receiver", address);

            Console.WriteLine("Please enter Message: ");
            Message helloOut, helloIn;
            String msg;

            try
            {
               while ((msg = Console.ReadLine()) != "q")
               {
                  switch (msg)
                  {
                     case ">":
                        msg = Console.ReadLine();
                        helloOut = new Message(msg);
                        sender.Send(helloOut);
                        Console.WriteLine(String.Format("Message: {0}; sent to {1}:{2}", helloOut.Body.ToString(), broker, address));
                        break;
                     case "<":
                        helloIn = receiver.Receive();
                        receiver.Accept(helloIn);
                        Console.WriteLine(String.Format("Message: {0}; recieved from {1}:{2}", helloIn.Body.ToString(), broker, address));
                        break;
                     default:
                        helloOut = new Message(new Jedi(97, "Razzorion Jaxx"));
                        sender.Send(helloOut);
                        Console.WriteLine(String.Format("Message: {0}; sent to {1}:{2}", helloOut.Body.ToString(), broker, address));
                        break;
                  }
               }
            }
            catch (AmqpException amqex)
            {
               Console.WriteLine(amqex.Message);
            }
            catch (ArgumentException argex)
            {
               Console.WriteLine(argex.Message);
            }
            catch (TimeoutException timex)
            {
               Console.WriteLine(timex.Message);
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }

            receiver.Close();
            sender.Close();
            session.Close();
            connection.Close();
         }
      }
   }
}
