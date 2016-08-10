using System;
using System.Threading.Tasks;
using Common.Utils;

namespace Common.TimeToLive
{
    public class TimeToLiveFunctionRequest<TResult>
    {
        public TimeSpan TimeToLive { get; set; }

        public Func<TResult> Function { get; set; }

        public Action<Task<TResult>> TimeToLiveElapsedPostAction { get; set; }

        public TimeToLiveFunctionRequest(TimeSpan timeToLive, Func<TResult> func, Action<Task<TResult>> timeToLiveElapsedPostAction = null)
        {
            Guard.ArgumentNotNull(func, "func");

            TimeToLive = timeToLive;
            Function = func;
            TimeToLiveElapsedPostAction = timeToLiveElapsedPostAction;
        }
    }
}
