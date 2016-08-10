using System;

namespace Common.TimeToLive
{
    public class TimeToLiveActionResponse
    {
        public TimeToLiveActionResponse(bool timeToLiveElapsed = false)
        {
            TimeToLiveElapsed = timeToLiveElapsed;
        }
        public bool TimeToLiveElapsed { get; set; }

        public TimeSpan TimeToLiveRemainingTime { get; set; }
    }
}
