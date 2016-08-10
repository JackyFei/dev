using Common.Messaging.Consumer;
using Common.Messaging.Publisher;

namespace Common.Messaging.ServiceBroker
{
    public class ServiceBrokerContext : IPublishContext
    {
        public string ConnectionString { get; set; }

        public string InitiatorService { get; set; }

        public string TargetService { get; set; }

        public string MessageContract { get; set; }

        public string MessageType { get; set; }
    }
}
