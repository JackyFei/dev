using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Messaging.Publisher;
using Common.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Common.Messaging.RabbitMq
{
    public class RabbitMqPublisher : IPublisher
    {
        private readonly ConnectionManager _connectionManager = new ConnectionManager();
        private bool _isDisposed;

        #region IPublisher
        public void Publish<T>(T message, IPublishContext context = null)
        {
            Guard.ArgumentNotNull(message, "message");
            Guard.RequireAttribute(message, typeof(SerializableAttribute), "message");
            Guard.InstanceOfType(context, "context", typeof(RabbitMqContext));

            var rabbitMqContext = (RabbitMqContext)context;
            Debug.Assert(rabbitMqContext != null, "rabbitMqContext != null");

            var connection = _connectionManager.GetConnection(rabbitMqContext);
            //using(var connection = new ConnectionFactory().CreateConnection(rabbitMqContext.HostName))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: rabbitMqContext.Queue,
                    durable: rabbitMqContext.Durable,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.BasicPublish(exchange: "",
                    routingKey: rabbitMqContext.Queue,
                    basicProperties: GetBasicProperties(rabbitMqContext),
                    body: GetBytes(message));
            }
        }

        public void Publish<T>(IEnumerable<T> messages, IPublishContext context = null)
        {
            Guard.ArgumentNotNull(messages, "messages");
            Guard.InstanceOfType(context, "context", typeof(RabbitMqContext));

            var rabbitMqContext = (RabbitMqContext)context;
            Debug.Assert(rabbitMqContext != null, "rabbitMqContext != null");

            var connection = _connectionManager.GetConnection(rabbitMqContext);
            //using (var connection = new ConnectionFactory().CreateConnection(rabbitMqContext.HostName))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: rabbitMqContext.Queue,
                    durable: rabbitMqContext.Durable,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                foreach (var message in messages)
                {
                    channel.BasicPublish(exchange: "",
                    routingKey: rabbitMqContext.Queue,
                    basicProperties: GetBasicProperties(rabbitMqContext),
                    body: GetBytes(message));
                }           
            }
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                // Dispose managed objects.
                //_connectionManager.Dispose();
            }

            _isDisposed = true;
        }
        #endregion
        #endregion
        
        private static IBasicProperties GetBasicProperties(RabbitMqContext rabbitMqContext)
        {
            var basicProperties = new BasicProperties { Persistent = rabbitMqContext.Persistent };
            return basicProperties;
        }

        private static byte[] GetBytes(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }
    }
}
