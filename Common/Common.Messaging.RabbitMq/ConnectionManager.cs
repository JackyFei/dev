using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Common.Messaging.RabbitMq
{
    internal class ConnectionManager : IDisposable
    {
        private readonly IDictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        private readonly object _lockObj = new object();
        private bool _isDisposed;

        /// <summary>
        /// Get existing open connection as IConnection is thread safe.
        /// </summary>
        public IConnection GetConnection(RabbitMqContext rabbitMqContext)
        {
            var connectionKey = BuildConnectionKey(rabbitMqContext);
            IConnection connection;
            if (_connections.ContainsKey(connectionKey))
            {
                connection = _connections[connectionKey];
                if (connection != null && connection.IsOpen)
                    return connection;
            }

            lock (_lockObj)
            {
                if (_connections.ContainsKey(connectionKey))
                {
                    connection = _connections[connectionKey];
                    if (connection != null && connection.IsOpen)
                    {
                        return connection;
                    }
                    else
                    {
                        _connections.Remove(connectionKey);
                    }
                }
                var connectionFactory = new ConnectionFactory
                {
                    HostName = rabbitMqContext.HostName,
                    RequestedChannelMax = ushort.MaxValue
                };
                connection = connectionFactory.CreateConnection();
                _connections.Add(connectionKey, connection);
                return connection;
            }
        }

        private static string BuildConnectionKey(RabbitMqContext rabbitMqContext)
        {
            return rabbitMqContext.HostName;
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
                foreach (var connection in _connections.Values)
                {
                    if (connection != null && connection.IsOpen)
                    {
                        connection.Close();
                    }
                }
            }

            _isDisposed = true;
        }
        #endregion
    }
}
