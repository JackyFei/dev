using Common.Messaging.Consumer;
using Common.Messaging.RabbitMq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Messaging
{
    [TestClass]
    public class RabbitMqConsumerTest : ConsumerTestBase
    {
        private readonly IConsumer<Message> _consumer = new RabbitMqConsumer<Message>();

        private readonly IConsumeContext _context = new RabbitMqContext
        {
            HostName = "localhost",
            Durable = true,
            Queue = "TestQueue",
            Persistent = true
        };

        protected override IConsumer<Message> Consumer { get { return _consumer; } }
        protected override IConsumeContext ConsumeContext { get { return _context; } }
    }
}
