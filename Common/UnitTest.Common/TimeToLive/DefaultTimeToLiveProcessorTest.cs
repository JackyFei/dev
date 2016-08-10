using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common.TimeToLive;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.TimeToLive
{
    [TestClass]
    public class DefaultTimeToLiveProcessorTest
    {
        private readonly ITimeToLiveProcessor _processor = new DefaultTimeToLiveProcessor();
        private const int ExecutionTimeMilliSeconds = 1000;

        [TestMethod]
        public void Action_RanToCompletion()
        {
            var response =
                _processor.Execute(
                    new TimeToLiveActionRequest(
                        TimeSpan.FromMilliseconds(ExecutionTimeMilliSeconds) + TimeSpan.FromMilliseconds(100), Action,
                        RanToCompletionPostAction));
            Trace.WriteLine("response returned.");
            Trace.WriteLine($"remaining time: {response.TimeToLiveRemainingTime.TotalMilliseconds}");
            Assert.IsFalse(response.TimeToLiveElapsed);
        }

        [TestMethod]
        public void Action_Running()
        {
            var response =
                _processor.Execute(
                    new TimeToLiveActionRequest(
                        TimeSpan.FromMilliseconds(ExecutionTimeMilliSeconds) - TimeSpan.FromMilliseconds(100), Action,
                        RanToCompletionPostAction));
            Trace.WriteLine("response returned.");
            Trace.WriteLine($"remaining time: {response.TimeToLiveRemainingTime.TotalMilliseconds}");
            Assert.IsTrue(response.TimeToLiveElapsed);
            Thread.Sleep(ExecutionTimeMilliSeconds + 200);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Action_RanToCompletion_Fault()
        {
            try
            {
                _processor.Execute(
                    new TimeToLiveActionRequest(
                        TimeSpan.FromMilliseconds(ExecutionTimeMilliSeconds) + TimeSpan.FromMilliseconds(100),
                        ActionWithException,
                        RanToCompletionPostAction));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Exception was thrown from ActionWithException.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void Action_Running_Fault()
        {
            var response =
                _processor.Execute(
                    new TimeToLiveActionRequest(
                        TimeSpan.FromMilliseconds(ExecutionTimeMilliSeconds) - TimeSpan.FromMilliseconds(500),
                        ActionWithException, RanToCompletionPostAction));
            Trace.WriteLine("response returned.");
            Assert.IsTrue(response.TimeToLiveElapsed);
            Thread.Sleep(ExecutionTimeMilliSeconds + 700);
        }

        [TestMethod]
        public void Function_RanToCompletion()
        {
            var response =
                _processor.Execute(
                    new TimeToLiveFunctionRequest<int>(
                        TimeSpan.FromMilliseconds(ExecutionTimeMilliSeconds) + TimeSpan.FromMilliseconds(100), Function,
                        RanToCompletionPostFunction));
            Trace.WriteLine("response returned.");
            Assert.IsFalse(response.TimeToLiveElapsed);
            Assert.AreEqual(1, response.FunctionResult);
        }

        [TestMethod]
        public void Function_Running()
        {
            var response =
                _processor.Execute(
                    new TimeToLiveFunctionRequest<int>(
                        TimeSpan.FromMilliseconds(ExecutionTimeMilliSeconds) - TimeSpan.FromMilliseconds(100), Function,
                        RanToCompletionPostFunction));
            Trace.WriteLine("response returned.");
            Assert.IsTrue(response.TimeToLiveElapsed);
            Thread.Sleep(ExecutionTimeMilliSeconds + 200);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Function_RanToCompletion_Fault()
        {
            try
            {
                _processor.Execute(
                    new TimeToLiveFunctionRequest<int>(
                        TimeSpan.FromMilliseconds(ExecutionTimeMilliSeconds) + TimeSpan.FromMilliseconds(100),
                        FunctionWithException,
                        RanToCompletionPostFunction));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Exception was thrown from FunctionWithException.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void Function_Running_Fault()
        {
            var response =
                _processor.Execute(
                    new TimeToLiveFunctionRequest<int>(
                        TimeSpan.FromMilliseconds(ExecutionTimeMilliSeconds) - TimeSpan.FromMilliseconds(500),
                        FunctionWithException, RanToCompletionPostFunction));
            Trace.WriteLine("response returned.");
            Assert.IsTrue(response.TimeToLiveElapsed);
            Thread.Sleep(ExecutionTimeMilliSeconds + 700);
        }

        private static void Action()
        {
            Trace.WriteLine("Action is executing.");
            Thread.Sleep(ExecutionTimeMilliSeconds);
            Trace.WriteLine("Action is complete.");
        }

        private static void RanToCompletionPostAction(Task task)
        {
            Trace.WriteLine("TimeToLiveElapsedPostAction is executing.");
            Thread.Sleep(ExecutionTimeMilliSeconds);
            Trace.WriteLine("TimeToLiveElapsedPostAction is complete.");
        }

        private static void RanToCompletionPostFunction<TResult>(Task<TResult> task)
        {
            Trace.WriteLine("TimeToLiveElapsedPostAction is executing.");
            Thread.Sleep(ExecutionTimeMilliSeconds);
            Trace.WriteLine("TimeToLiveElapsedPostAction is complete.");
            Trace.WriteLine($"Function result: {task.Result}");
        }

        private static void ActionWithException()
        {
            Trace.WriteLine("ActionWithException is executing.");
            Thread.Sleep(ExecutionTimeMilliSeconds);
            throw new Exception("Exception was thrown from ActionWithException.");
        }


        private static int Function()
        {
            Trace.WriteLine("Function is executing.");
            Thread.Sleep(ExecutionTimeMilliSeconds);
            Trace.WriteLine("Function is complete.");
            return 1;
        }


        private static int FunctionWithException()
        {
            Trace.WriteLine("FunctionWithException is executing.");
            Thread.Sleep(ExecutionTimeMilliSeconds);
            throw new Exception("Exception was thrown from FunctionWithException.");
        }
    }
}

