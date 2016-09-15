using FamilyBooks.BusinessLogic.Repository;
using FamilyBooks.BusinessLogic.Validations;

namespace FamilyBooks.BusinessLogic.Record
{
    public class RecordManager
    {
        private readonly IValidationManager _validationManager;
        private readonly IFamilyBooksRepository _familyBooksRepository;

        public int RecordExpenditure(Expenditure expenditure)
        {
            _validationManager.ValidateExpenditure(expenditure);
            return _familyBooksRepository.CreateRecord(expenditure);
        }
    }
}
