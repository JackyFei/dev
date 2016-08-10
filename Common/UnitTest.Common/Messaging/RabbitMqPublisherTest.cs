using System;
using System.Collections.Generic;
using Common.Messaging.Publisher;
using Common.Messaging.RabbitMq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Messaging
{
    [TestClass]
    public class RabbitMqPublisherTest : PublisherTestBase
    {
        private readonly IPublisher _publisher = new RabbitMqPublisher();

        private readonly IPublishContext _context = new RabbitMqContext
        {
            HostName = "localhost",
            Queue = "TestQueue",
            Durable = true,
            Persistent = true
        };

        protected override IPublisher Publisher
        {
            get
            {
                return _publisher;
            }
        }

        protected override IPublishContext PublishContext
        {
            get { return _context; }
        }
    }
}
