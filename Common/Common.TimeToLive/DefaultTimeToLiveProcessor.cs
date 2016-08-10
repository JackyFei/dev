using System;
using System.Threading.Tasks;
using Common.Utils;

namespace Common.TimeToLive
{
    public class DefaultTimeToLiveProcessor : ITimeToLiveProcessor
    {
        public TimeToLiveActionResponse Execute(TimeToLiveActionRequest request)
        {
            Guard.ArgumentNotNull(request, "request");

            var executionStartDateTime = DateTime.Now;

            var response = new TimeToLiveActionResponse();

            var timeToLive = request.TimeToLive;
            if (timeToLive <= TimeSpan.Zero)
            {
                // if timeToLiveMilliSeconds is not greater than 0, just execute the action in same thread, no ttl.
                request.Action.Invoke();
                return response;
            }

            var task = new Task(request.Action);
            task.Start();

            try
            {
                task.Wait(timeToLive);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
            }

            switch (task.Status)
            {
                case TaskStatus.Created:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    response.TimeToLiveElapsed = true;
                    // Executed only when the action is complete.
                    if (request.TimeToLiveElapsedPostAction != null)
                    {
                        task.ContinueWith(request.TimeToLiveElapsedPostAction, TaskContinuationOptions.OnlyOnRanToCompletion);
                    }
                    break;
                case TaskStatus.RanToCompletion:
                    response.TimeToLiveElapsed = false;
                    break;
                case TaskStatus.Faulted:
                    var ex = task.Exception;
                    if (ex?.InnerException != null)
                        throw ex.InnerException;
                    break;
                case TaskStatus.Canceled:
                    throw new Exception($"{request.Action.Method.Name} was canceled.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            response.TimeToLiveRemainingTime = GetRemainingTimeToLive(
                request.TimeToLive, executionStartDateTime);
            return response;
        }

        public TimeToLiveFunctionResponse<TResponse> Execute<TResponse>(TimeToLiveFunctionRequest<TResponse> request)
        {
            Guard.ArgumentNotNull(request, "request");

            var executionStartDateTime = DateTime.Now;
            var response = new TimeToLiveFunctionResponse<TResponse>();

            var timeToLive = request.TimeToLive;
            if (timeToLive <= TimeSpan.Zero)
            {
                // if timeToLive is not greater than 0, just execute the action in same thread, no ttl.
                response.FunctionResult = request.Function.Invoke();
                return response;
            }

            var task = new Task<TResponse>(request.Function);
            task.Start();

            try
            {
                task.Wait(timeToLive);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
            }

            switch (task.Status)
            {
                case TaskStatus.Created:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    response.TimeToLiveElapsed = true;
                    if (request.TimeToLiveElapsedPostAction != null)
                    {
                        task.ContinueWith(request.TimeToLiveElapsedPostAction, TaskContinuationOptions.OnlyOnRanToCompletion);
                    }
                    break;
                case TaskStatus.RanToCompletion:
                    response.FunctionResult = task.Result;
                    response.TimeToLiveElapsed = false;
                    break;
                case TaskStatus.Faulted:
                    var ex = task.Exception;
                    if (ex?.InnerException != null)
                        throw ex.InnerException;
                    break;
                case TaskStatus.Canceled:
                    throw new Exception($"{request.Function.Method.Name} was canceled.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            response.TimeToLiveRemainingTime = GetRemainingTimeToLive(
                request.TimeToLive, executionStartDateTime);
            return response;
        }

        private static TimeSpan GetRemainingTimeToLive(TimeSpan timeToLive, DateTime executionStartDatetime)
        {
            var executionTime = DateTime.Now - executionStartDatetime;
            var ttlRemainingTime = timeToLive - executionTime;
            return ttlRemainingTime < TimeSpan.Zero ? TimeSpan.Zero : ttlRemainingTime;
        }
    }
}
