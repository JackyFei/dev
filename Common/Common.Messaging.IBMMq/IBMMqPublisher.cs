using System;
using System.Collections;
using System.Collections.Generic;
using Common.Messaging.Publisher;
using IBM.WMQ;

namespace Common.Messaging.IBMMq
{
    public class IBMMqPublisher : IPublisher
    {
        #region IPublisher
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(T message, IPublishContext context = null)
        {
            var connectionProperties = new Hashtable
            {
                {MQC.HOST_NAME_PROPERTY, "gdcdevqamq01"},
                {MQC.PORT_PROPERTY, 1420},
                {MQC.CHANNEL_PROPERTY, "GDC.SSL.DV1.INTERNAL"},
                {
                    MQC.TRANSPORT_PROPERTY,
                    MQC.TRANSPORT_MQSERIES_MANAGED
                }
            };

            try
            {
                var mqQMgr = new MQQueueManager("QMGR.GDCDV1", connectionProperties);
            }
            catch (MQException mqe)
            {
                
                return;
            }
        }

        public void Publish<T>(IEnumerable<T> messages, IPublishContext context = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
