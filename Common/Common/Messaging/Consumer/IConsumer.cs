using System;

namespace Common.Messaging.Consumer
{
    public interface IConsumer<out T> : IDisposable
    {
        event Action<T> OnMessageReceived;

        void Start(IConsumeContext consumeContext = null);

        void Stop();
    }

    public interface IConsumeContext { }
}
