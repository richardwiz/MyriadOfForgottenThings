//  ------------------------------------------------------------------------------------
//  Copyright (c) 2015 Red Hat, Inc.
//  All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the ""License""); you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//  EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR
//  CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR
//  NON-INFRINGEMENT.
//
//  See the Apache Version 2.0 License for specific language governing permissions and
//  limitations under the License.
//  ------------------------------------------------------------------------------------

//
// HelloWorld_robust
//
// Command line:
//   HelloWorld_robust [brokerUrl [brokerEndpointAddress [payloadText [enableTrace]]]]
//
// Default:
//   HelloWorld_robust amqp://localhost:5672 amq.topic
//
// Requires:
//   An unauthenticated broker or peer at the brokerUrl
//   capable of receiving and delivering messages through
//   the endpoint address.
//
using System;
using System.Threading;
using Amqp;
using Amqp.Framing;
using Amqp.Types;

namespace HelloWorld_robust
{
    class HelloWorld_robust
    {
        static void PrintMessage(Message message)
        {
            if (message.Header != null) Console.WriteLine(message.Header.ToString());
            if (message.DeliveryAnnotations != null) Console.WriteLine(message.DeliveryAnnotations.ToString());
            if (message.MessageAnnotations != null) Console.WriteLine(message.MessageAnnotations.ToString());
            if (message.Properties != null) Console.WriteLine(message.Properties.ToString());
            if (message.ApplicationProperties != null) Console.WriteLine(message.ApplicationProperties.ToString());
            if (message.BodySection != null) Console.WriteLine("body:{0}", message.Body.ToString());
            if (message.Footer != null) Console.WriteLine(message.Footer.ToString());
        }

        static int Main(string[] args)
        {
            string broker = args.Length >= 1 ? args[0] : "amqp://localhost:5672";
            string address = args.Length >= 2 ? args[1] : "MarioBros";
            string payload = args.Length >= 3 ? args[2] : "Hello World!";
            bool logging = args.Length >= 4;
            int exitStatus = 0;

            Console.WriteLine("Broker: {0}, Address: {1}, Payload: {2}", broker, address, payload);

            Connection.DisableServerCertValidation = true;

            if (logging)
            {
                Trace.TraceLevel = TraceLevel.Frame;
                Trace.TraceListener = (f, a) => Console.WriteLine(DateTime.Now.ToString("[hh:mm:ss.fff]") + " " + string.Format(f, a));
            }

            Connection connection = null;
            try
            {
                Address brokerAddr = new Address(broker);
                connection = new Connection(brokerAddr);
                Session session = new Session(connection);

                SenderLink sender = new SenderLink(session, "helloworld-sender", address);
                ReceiverLink receiver = new ReceiverLink(session, "helloworld-receiver", address);

                Message helloOut = new Message(payload);
                sender.Send(helloOut);

                Message helloIn = receiver.Receive();
                receiver.Accept(helloIn);

                PrintMessage(helloIn);

                sender.Close();
                receiver.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (null != connection)
                {
                    connection.Close();
                }
                exitStatus = 1;
            }
         Console.ReadLine();
            return exitStatus;
        }
    }
}
