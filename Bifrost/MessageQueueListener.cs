using Amqp;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MessageQueueListener
{
    //private readonly IResultsRepository _resultsRepository;

    public MessageQueueListener(/*IResultsRepository resultsRepository*/)
    {
        //_resultsRepository = resultsRepository ?? throw new System.ArgumentNullException(nameof(resultsRepository));
    }

    public void Start(string amqpBrokerAddress)
    {
        Task.Run(() => StartListener(amqpBrokerAddress));
    }

    private void StartListener(string amqpBrokerAddress)
    {
        const string recieverQueue = "tags.txns::tags.testqueue1";
        while (true)
        {
            try
            {
                //Log.Information("Connecting to AMQP broker {AmqpBrokerAddress}", amqpBrokerAddress);
                Address address = new Address(amqpBrokerAddress);
                Connection connection = new Connection(address);
                Session session = new Session(connection);
                ListenOnQueue(recieverQueue, session);
                    
                session.Close();
                connection.Close();
            }
            catch (Exception e)
            {
    //            Log.Error(e, "Connection failed to broker: {ErrorMessage}", e.Message);
				//Log.Debug("Waiting 5 seconds to reconnect...");
				Thread.Sleep(5000);
            }
        }
    }

    private void ListenOnQueue(string recieverQueue, Session session)
    {
        //using (LogContext.PushProperty("QueueName", recieverQueue))
        //{
        //    ReceiverLink receiver = new ReceiverLink(session, "receiver-link", recieverQueue);
        //    Log.Information("Receiver connected to broker.");

        //    while (!receiver.IsClosed)
        //    {
        //        Message message = receiver.Receive();
        //        if (message == null) continue;
                    
        //        //if (message.Properties.Subject == typeof(TxKdcDrawResults).FullName)
        //        //{
        //        //    string jsonMessage = message.GetBody<string>();
        //        //    //var kenoDrawResults = jsonMessage.TxDeserialize<TxKdcDrawResults>();
        //        //    //Log.Information("Received {@KenoDrawResults}", kenoDrawResults);
        //        //    _resultsRepository.AddKenoResult(kenoDrawResults.ToKenoDrawResults());
        //        //}

        //        receiver.Accept(message);
        //    }

            //receiver.Close(TimeSpan.Zero);
        //}
    }
}