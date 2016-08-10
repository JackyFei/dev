using Common.Messaging.Consumer;

namespace Common.Messaging.ServiceBroker
{
    public class ServiceBrokerConsumeContext : IConsumeContext
    {
        public string ConnectionString { get; set; }
        public string Queue { get; set; }
    }
}
