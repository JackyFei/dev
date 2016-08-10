using Common.Messaging.Consumer;
using Common.Messaging.Publisher;

namespace Common.Messaging.RabbitMq
{
    public class RabbitMqContext : IPublishContext, IConsumeContext
    {
        public string HostName { get; set; }

        public string Queue { get; set; }

        public bool Durable { get; set; }

        public bool Persistent { get; set; }
    }
}
