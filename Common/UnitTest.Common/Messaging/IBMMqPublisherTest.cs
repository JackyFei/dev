using Common.Messaging.IBMMq;
using Common.Messaging.Publisher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Messaging
{
    [TestClass]
    public class IBMMqPublisherTest : PublisherTestBase
    {
        private readonly IPublisher _publisher = new IBMMqPublisher();
        private readonly IPublishContext _context = new IBMMqContext();
        protected override IPublisher Publisher {get { return _publisher; } }
        protected override IPublishContext PublishContext { get { return _context; } }
    }
}
