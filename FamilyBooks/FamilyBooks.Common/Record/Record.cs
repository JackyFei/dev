using System;
using System.Runtime.Serialization;

namespace FamilyBooks.Common.Record
{
    public abstract class Record
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }
        [DataMember(Name = "Amount")]
        public decimal Amount { get; set; }
        [DataMember(Name = "Date")]
        public DateTimeOffset DateTimeOffset { get; set; }
        [DataMember(Name = "Comment")]
        public string Comment { get; set; }
        [DataMember(Name = "AccountID")]
        public string AccountID { get; set; }
        [DataMember(Name = "CategoryID")]
        public string CategoryID { get; set; }
    }
}
