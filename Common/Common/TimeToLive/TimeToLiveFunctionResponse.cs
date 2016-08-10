
namespace Common.TimeToLive
{
    public class TimeToLiveFunctionResponse<TResult> : TimeToLiveActionResponse
    {
        public TResult FunctionResult { get; set; }

        public TimeToLiveFunctionResponse(bool timeToLiveElapsed = false) : base(timeToLiveElapsed)
        {
        }
    }
}
