using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Messaging.Publisher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Messaging
{
    [TestClass]
    public abstract class PublisherTestBase
    {
        protected abstract IPublisher Publisher { get; }

        protected abstract IPublishContext PublishContext { get; }

        [ClassCleanup]
        public void Cleanup()
        {
            Publisher.Dispose();
        }

        [TestMethod]
        public void Publish_MultipleMessages_Success()
        {
            var context = PublishContext;

            var messages = new Message[500];
            for (int i = 0; i < 500; i++)
            {
                messages[i] = new Message
                {
                    ExecutionDateTime = DateTime.Now,
                    Name = "Message " + new Random().Next()
                };
            }

            Publisher.Publish((IEnumerable<Message>)messages, context);
        }

        [TestMethod]
        public void Publish_SingleMessage_MultipleTimes_Success()
        {
            var context = PublishContext;

            for (var i = 0; i < 500; i++)
            {
                var message = new Message
                {
                    ExecutionDateTime = DateTime.Now,
                    Name = "Message " + new Random().Next()
                };

                Publisher.Publish(message, context);
            }
        }

        [TestMethod]
        public void Publish_SingleMessage_MultipleTimes_MultipleThread_Success()
        {
            var tasks = new Task[20];
            for (var i = 0; i < 20; i++)
            {
                var context = PublishContext;
                tasks[i] = Task.Run(() =>
                {
                    for (var j = 0; j < 25; j++)
                    {
                        var message = new Message
                        {
                            ExecutionDateTime = DateTime.Now,
                            Name = "Message " + new Random().Next()
                        };

                        Publisher.Publish(message, context);
                    }
                });
            }
            Task.WaitAll(tasks);
        }
    }
}