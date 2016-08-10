using Common.Messaging.Publisher;

namespace Common.Messaging.IBMMq
{
    public class IBMMqContext : IPublishContext
    {
        public string QueueManager { get; set; }
    }
}
