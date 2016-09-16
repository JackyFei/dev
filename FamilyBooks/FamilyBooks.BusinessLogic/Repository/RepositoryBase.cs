using Common.Data.SQL;

namespace FamilyBooks.BusinessLogic.Repository
{
    public abstract class RepositoryBase
    {
        protected readonly IDataAccess DataAccess;

        protected RepositoryBase()
        {
            DataAccess = new EnterpriseLibraryDataAccess("FamilyBooks");
        }
    }
}