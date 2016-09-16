using FamilyBooks.BusinessLogic.Record;

namespace FamilyBooks.BusinessLogic.Repository
{
    public interface IFamilyBooksRepository
    {
        int CreateRecord(RecordBase record);
        int UpdateRecord(RecordBase record);
    }
}
