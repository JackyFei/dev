using System;

namespace FamilyBooks.BusinessLogic.Record
{
    public abstract class RecordBase
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public string Comment { get; set; }
        public int AccountID { get; set; }
        public int CategoryID { get; set; }
        public abstract RecordType RecordType { get; }
    }
}