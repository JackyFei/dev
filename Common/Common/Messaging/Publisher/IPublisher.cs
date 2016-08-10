using System;
using System.Collections.Generic;

namespace Common.Messaging.Publisher
{
    public interface IPublisher : IDisposable
    {
        void Publish<T>(T message, IPublishContext context = null);
        void Publish<T>(IEnumerable<T> messages, IPublishContext context = null);
    }

    public interface IPublishContext
    {
        
    }
}
