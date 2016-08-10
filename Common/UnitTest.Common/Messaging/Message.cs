using System;

namespace UnitTest.Common.Messaging
{
    [Serializable]
    public class Message
    {
        public DateTime ExecutionDateTime { get; set; }
        public string Name { get; set; }
    }
}
