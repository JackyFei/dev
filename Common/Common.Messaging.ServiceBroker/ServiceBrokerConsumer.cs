using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Common.Messaging.Consumer;
using Common.Utils;
using ServiceBroker.Net;

namespace Common.Messaging.ServiceBroker
{
    public class ServiceBrokerConsumer<T> : IConsumer<T>
    {
        private bool _isStarted = false;
        private bool _isDisposed = false;
        private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();

        #region IConsumer

        public event Action<T> OnMessageReceived;

        public void Start(IConsumeContext consumeContext = null)
        {
            Guard.InstanceOfType(consumeContext, "consumeContext", typeof(ServiceBrokerConsumeContext));

            SetIsStarted(true);

            var serviceBrokerContext = (ServiceBrokerConsumeContext) consumeContext;

            // ReSharper disable once PossibleNullReferenceException
            using (var sqlConnection = new SqlConnection(serviceBrokerContext.ConnectionString))
            {
                sqlConnection.Open();
                while (GetIsStarted())
                {
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        IEnumerable<Message> messages = null;
                        while (messages == null)
                        {
                            messages = ServiceBrokerWrapper.WaitAndReceive(sqlTransaction, serviceBrokerContext.Queue, 1000);
                            foreach (var message in messages)
                            {
                                if (message.MessageTypeName == "VoidMessage")
                                {
                                    var formatter = new BinaryFormatter();
                                    message.BodyStream.Position = 0;
                                    var obj = (T)formatter.Deserialize(message.BodyStream);

                                    if (OnMessageReceived != null)
                                    {
                                        try
                                        {
                                            OnMessageReceived(obj);
                                            ServiceBrokerWrapper.EndConversation(sqlTransaction,
                                                message.ConversationHandle,
                                                serviceBrokerContext.Queue);
                                        }
                                        catch (Exception ex)
                                        {
                                            Trace.TraceError(ex.Message);
                                        }        
                                    }      
                                }
                                else
                                {
                                    Trace.WriteLine("Received message: " + message.MessageTypeName + " " +
                                                    message.ConversationHandle);
                                }                              
                            }
                        }
                        sqlTransaction.Commit();
                    }
                }
            }
        }

        public void Stop()
        {
            SetIsStarted(false);
        }

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
                if (_lockSlim != null)
                {
                    _lockSlim.Dispose();
                }
            }

            _isDisposed = true;
        }

        #endregion

        private void SetIsStarted(bool isStarted)
        {
            try
            {
                _lockSlim.EnterWriteLock();
                _isStarted = isStarted;
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }

        private bool GetIsStarted()
        {
            try
            {
                _lockSlim.EnterReadLock();
                return _isStarted;
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }
    }
}
