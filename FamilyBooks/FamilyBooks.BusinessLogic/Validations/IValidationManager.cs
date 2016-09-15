using FamilyBooks.BusinessLogic.Record;

namespace FamilyBooks.BusinessLogic.Validations
{
    public interface IValidationManager
    {
        void ValidateExpenditure(Expenditure expenditure);
    }
}
