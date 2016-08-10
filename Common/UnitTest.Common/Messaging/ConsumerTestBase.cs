using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common.Messaging.Consumer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Messaging
{
    [TestClass]
    public abstract class ConsumerTestBase
    {
        protected abstract IConsumer<Message> Consumer { get; }

        protected abstract IConsumeContext ConsumeContext { get; }

        [TestMethod]
        public void Consume_Success()
        {
            var context = ConsumeContext;

            Consumer.OnMessageReceived += message =>
            {
                var now = DateTime.Now;
                Trace.WriteLine(now.ToString("hh:mm:ss fff") + ": " + message.Name + " " + message.ExecutionDateTime);
            };

            // ReSharper disable once AccessToDisposedClosure
            Task.Factory.StartNew(() => Consumer.Start(context));
            
            Thread.Sleep(10000);
            Consumer.Stop();
            Consumer.Dispose();
        }
    }
}
