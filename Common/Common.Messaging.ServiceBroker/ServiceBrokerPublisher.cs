using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Messaging.Publisher;
using Common.Utils;
using ServiceBroker.Net;

namespace Common.Messaging.ServiceBroker
{
    public class ServiceBrokerPublisher : IPublisher
    {
        #region IPublisher
        public void Publish<T>(T message, IPublishContext context = null)
        {
            Guard.ArgumentNotNull(message, "message");
            Guard.InstanceOfType(context, "context", typeof(ServiceBrokerContext));
            var serviceBrokerContext = (ServiceBrokerContext) context;

            byte[] buffer;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, message);
                buffer = stream.GetBuffer();
                stream.Close();
            }

            // ReSharper disable once PossibleNullReferenceException
            using (var sqlConnection = new SqlConnection(serviceBrokerContext.ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    var conversationHandle = ServiceBrokerWrapper.BeginConversation(sqlTransaction,
                        serviceBrokerContext.InitiatorService, serviceBrokerContext.TargetService,
                        serviceBrokerContext.MessageContract, false);

                    ServiceBrokerWrapper.Send(sqlTransaction, conversationHandle, serviceBrokerContext.MessageType,
                        buffer);
                  
                    sqlTransaction.Commit();
                }
            }
        }

        public void Publish<T>(IEnumerable<T> messages, IPublishContext context = null)
        {
            Guard.ArgumentNotNull(messages, "message");
            Guard.InstanceOfType(context, "context", typeof(ServiceBrokerContext));
            var serviceBrokerContext = (ServiceBrokerContext)context;

            // ReSharper disable once PossibleNullReferenceException
            using (var sqlConnection = new SqlConnection(serviceBrokerContext.ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    var conversationHandle = ServiceBrokerWrapper.BeginConversation(sqlTransaction,
                        serviceBrokerContext.InitiatorService, serviceBrokerContext.TargetService,
                        serviceBrokerContext.MessageContract, false);

                    foreach (var message in messages)
                    {
                        byte[] buffer;
                        using (var stream = new MemoryStream())
                        {
                            var formatter = new BinaryFormatter();
                            formatter.Serialize(stream, message);
                            buffer = stream.GetBuffer();
                            stream.Close();
                        }
                        ServiceBrokerWrapper.Send(sqlTransaction, conversationHandle, serviceBrokerContext.MessageType,
                        buffer);
                    }

                    sqlTransaction.Commit();
                }
            }
        }

        public void Dispose()
        {
            
        }
        #endregion
    }
}
