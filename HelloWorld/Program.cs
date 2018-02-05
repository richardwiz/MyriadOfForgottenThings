/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements. See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Amqp;
namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
         string broker = args.Length >= 1 ? args[0] : "amqp://localhost:5672";
         string address = args.Length >= 2 ? args[1] : "amq.topic";

         Address brokerAddr = new Address(broker);
         Connection connection = new Connection(brokerAddr);
         Session session = new Session(connection);

         SenderLink sender = new SenderLink(session, "helloworld-sender", address);
         ReceiverLink receiver = new ReceiverLink(session, "helloworld-receiver", address);

         Message helloOut = new Message("Hello World!");
         sender.Send(helloOut);

         Message helloIn = receiver.Receive();
         receiver.Accept(helloIn);

         Console.WriteLine(helloIn.Body.ToString());
         Console.Read();

         receiver.Close();
         sender.Close();
         session.Close();
         connection.Close();
      }
    }
}
