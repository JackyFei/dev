using Common.Utils;
using FamilyBooks.Common.Record;

namespace FamilyBooks.Facade
{
    internal class Converter
    {
        internal BusinessLogic.Record.Expenditure Convert(Expenditure expenditure)
        {
            Guard.ArgumentNotNull(expenditure, "expenditure");
            var modelExpenditure = new BusinessLogic.Record.Expenditure { };
            return modelExpenditure;
        }
    }
}
