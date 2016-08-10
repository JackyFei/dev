using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Messaging.Consumer;
using Common.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Messaging.RabbitMq
{
    public class RabbitMqConsumer<T> : IConsumer<T>
    {
        private IConnection _connection;
        private IModel _channel;
        private bool _isDisposed;
        private readonly object _lockObj = new object();
    

        #region IConsumer
        public event Action<T> OnMessageReceived;

        public void Start(IConsumeContext consumeContext)
        {
            Guard.InstanceOfType(consumeContext, "consumeContext", typeof(RabbitMqContext));

            Stop();

            try
            {
                var rabbitMqContext = (RabbitMqContext)consumeContext;
                _connection = new ConnectionFactory().CreateConnection(rabbitMqContext.HostName);
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queue: rabbitMqContext.Queue,
                                         durable: rabbitMqContext.Durable,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var obj = DeserializeBody(ea.Body);
                        var message = (T)obj;
                        if (OnMessageReceived != null)
                        {
                            OnMessageReceived(message);
                            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Error when executing action. ea.Tab: " + ea.DeliveryTag + " " + ex.Message);
                    }
                };

                _channel.BasicConsume(queue: rabbitMqContext.Queue, noAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                Stop();
                Trace.TraceError("Error when consuming messages" + ex.Message);
            }
            
        }

        public void Stop()
        {
            lock (_lockObj)
            {
                if (_channel != null)
                {
                    if (_channel.IsOpen)
                    {
                        _channel.Close();
                    }
                    _channel.Dispose();
                }

                if (_connection != null)
                {
                    if (_connection.IsOpen)
                    {
                        _connection.Close();
                    }
                    _connection.Dispose();
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
                Stop();
            }

            _isDisposed = true;
        }
        #endregion
        #endregion

        private static object DeserializeBody(byte[] body)
        {
            var memoryStream = new MemoryStream();
            using (var streamWriter = new BinaryWriter(memoryStream))
            {
                streamWriter.Write(body);
                streamWriter.Flush();
                memoryStream.Position = 0;
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(memoryStream);
            }
        }
    }
}
