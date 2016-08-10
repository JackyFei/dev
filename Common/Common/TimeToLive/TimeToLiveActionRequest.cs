using System;
using System.Threading.Tasks;
using Common.Utils;

namespace Common.TimeToLive
{
    public class TimeToLiveActionRequest
    {
        public TimeToLiveActionRequest(TimeSpan timeToLive, Action action, Action<Task> timeToLiveElapsedPostAction = null)
        {
            Guard.ArgumentNotNull(action, "action");

            TimeToLive = timeToLive;
            Action = action;
            TimeToLiveElapsedPostAction = timeToLiveElapsedPostAction;
        }

        public TimeSpan TimeToLive { get; private set; }

        public Action Action { get; private set; }

        public Action<Task> TimeToLiveElapsedPostAction { get; set; }
    }
}
