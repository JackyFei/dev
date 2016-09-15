using FamilyBooks.BusinessLogic.Record;
using Common.Utils;

namespace FamilyBooks.BusinessLogic.Validations
{
    public class ValidationManager : IValidationManager
    {
        public void ValidateExpenditure(Expenditure expenditure)
        {
            Guard.ArgumentNotNull(expenditure, "expenditure");
        }
    }
}
