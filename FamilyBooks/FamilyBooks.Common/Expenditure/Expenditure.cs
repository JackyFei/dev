using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyBooks.Common.Expenditure
{
    public class Expenditure
    {
        public string ID { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenditureDate { get; set; }

        public string Comment { get; set; }

        public string ExpenditureCategoryID { get; set; }

        public string AccountID { get; set; }
    }
}
