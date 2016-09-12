using System.Runtime.Serialization;

namespace FamilyBooks.Common.Account
{
    [DataContract(Name = "AccountType", Namespace = Consts.Namespace)]
    public class AccountType
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }
        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}