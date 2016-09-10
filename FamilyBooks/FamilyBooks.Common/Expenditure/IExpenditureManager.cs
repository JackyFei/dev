using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyBooks.Common.Expenditure
{
    public interface IExpenditureManager
    {
        string RecordExpenditure(Expenditure expenditure);
        bool UpdateExpenditure(Expenditure expenditure);
        bool DeleteExpenditure(string expenditureID);
    }
}
