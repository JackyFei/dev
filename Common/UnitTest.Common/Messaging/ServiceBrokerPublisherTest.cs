using Common.Messaging.Publisher;
using Common.Messaging.ServiceBroker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Messaging
{
    [TestClass]
    public class ServiceBrokerPublisherTest: PublisherTestBase
    {
        private readonly IPublisher _publisher = new ServiceBrokerPublisher();

        private readonly IPublishContext _context = new ServiceBrokerContext
        {
            ConnectionString = "Data Source=SHA-JFEI-L1\\SQLEXPRESS;Initial Catalog=ServiceBrokerResearch;Integrated Security=True",
            InitiatorService = "VoidReplyService",
            TargetService = "VoidService",
            MessageContract = "VoidContract",
            MessageType = "VoidMessage"
        };

        protected override IPublisher Publisher { get {return _publisher;} }

        protected override IPublishContext PublishContext
        {
            get
            {
                return _context;
            }
        }
    }
}
