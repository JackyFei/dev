using Common.Utils;
using FamilyBooks.BusinessLogic.Repository;
using FamilyBooks.BusinessLogic.Validations;

namespace FamilyBooks.BusinessLogic.Record
{
    public class RecordManager
    {
        private readonly IFamilyBooksRepository _familyBooksRepository;
        private readonly IValidationManager _validationManager;

        public RecordManager(IFamilyBooksRepository familyBooksRepository, IValidationManager validationManager)
        {
            Guard.ArgumentNotNull(familyBooksRepository, "familyBooksRepository");
            Guard.ArgumentNotNull(validationManager, "validationManager");
            _familyBooksRepository = familyBooksRepository;
            _validationManager = validationManager;
        }

        public int RecordExpenditure(Expenditure expenditure)
        {
            _validationManager.ValidateExpenditure(expenditure);
            return _familyBooksRepository.CreateRecord(expenditure);
        }
    }
}
