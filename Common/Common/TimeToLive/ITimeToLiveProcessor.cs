
namespace Common.TimeToLive
{
    public interface ITimeToLiveProcessor
    {
        TimeToLiveActionResponse Execute(TimeToLiveActionRequest request);
        TimeToLiveFunctionResponse<TResponse> Execute<TResponse>(TimeToLiveFunctionRequest<TResponse> request);
    }
}
