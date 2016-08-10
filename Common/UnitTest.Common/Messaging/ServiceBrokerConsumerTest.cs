using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common.Messaging.Consumer;
using Common.Messaging.RabbitMq;
using Common.Messaging.ServiceBroker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Messaging
{
    [TestClass]
    public class ServiceBrokerConsumerTest : ConsumerTestBase
    {
        private readonly IConsumer<Message> _consumer = new ServiceBrokerConsumer<Message>();

        private readonly IConsumeContext _context = new ServiceBrokerConsumeContext()
        {
            ConnectionString = "Data Source=SHA-JFEI-L1\\SQLEXPRESS;Initial Catalog=ServiceBrokerResearch;Integrated Security=True",
            Queue = "VoidQueue"
        };

        protected override IConsumer<Message> Consumer { get { return _consumer; } }
        protected override IConsumeContext ConsumeContext { get { return _context; } }
    }
}
