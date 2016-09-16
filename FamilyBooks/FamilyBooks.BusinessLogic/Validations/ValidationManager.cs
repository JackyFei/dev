using FamilyBooks.BusinessLogic.Record;
using Common.Utils;
using FamilyBooks.BusinessLogic.Exceptions;

namespace FamilyBooks.BusinessLogic.Validations
{
    public class ValidationManager : IValidationManager
    {
        public void ValidateExpenditure(Expenditure expenditure)
        {
            Guard.ArgumentNotNull(expenditure, "expenditure");
            if (expenditure.Amount >= 0)
            {
                throw new ValidationException("Amount of Expenditure should be less than zero.", ValidationErrorCode.InvalidAmount);
            }
        }
    }
}
