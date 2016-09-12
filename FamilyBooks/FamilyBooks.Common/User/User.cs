using System.Runtime.Serialization;

namespace FamilyBooks.Common.User
{
    [DataContract(Name = "User", Namespace = Consts.Namespace)]
    public class User
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Login")]
        public string Login { get; set; }

        [DataMember(Name = "PasswordHash")]
        public string PasswordHash { get; set; }
    }
}