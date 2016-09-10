using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyBooks.Common.Account
{
    public class Account
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string AccountTypeID { get; set; }

        public string UserID { get; set; }
    }
}
