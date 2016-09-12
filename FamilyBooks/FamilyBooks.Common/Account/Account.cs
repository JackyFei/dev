using System.Runtime.Serialization;

namespace FamilyBooks.Common.Account
{
    [DataContract(Name = "Account", Namespace = Consts.Namespace)]
    public class Account
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        [DataMember(Name = "IsActive")]
        public bool IsActive { get; set; }
        [DataMember(Name = "AccountTypeID")]
        public string AccountTypeID { get; set; }
        [DataMember(Name = "UserID")]
        public string UserID { get; set; }
    }
}