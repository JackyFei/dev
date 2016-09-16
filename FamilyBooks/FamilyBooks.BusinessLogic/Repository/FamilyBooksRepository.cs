using System;
using System.Data;
using FamilyBooks.BusinessLogic.Record;

namespace FamilyBooks.BusinessLogic.Repository
{
    public class FamilyBooksRepository : RepositoryBase, IFamilyBooksRepository
    {
        public int CreateRecord(RecordBase record)
        {
            using (var cmd = DataAccess.GetStoredProcCommand("dbo.CreateRecord"))
            {
                DataAccess.AddInParameter(cmd, "@pAccountID", DbType.Int32, record.AccountID);
                DataAccess.AddInParameter(cmd, "@pRecordType", DbType.Boolean, record.RecordType);
                DataAccess.AddInParameter(cmd, "@pRecordCategoryID", DbType.Int32, record.CategoryID);
                DataAccess.AddInParameter(cmd, "@pAmount", DbType.Decimal, record.Amount);
                DataAccess.AddInParameter(cmd, "@pDateTimeOffset", DbType.DateTimeOffset, record.DateTimeOffset);
                DataAccess.AddInParameter(cmd, "@pComment", DbType.String, record.Comment);
                var obj = DataAccess.ExecuteScalar(cmd);
                return Convert.ToInt32(obj);
            }
        }

        public int UpdateRecord(RecordBase record)
        {
            if (record.ID < 1)
                return 0;

            using (var cmd = DataAccess.GetStoredProcCommand("dbo.UpdateRecord"))
            {
                DataAccess.AddInParameter(cmd, "@pRecordID", DbType.Int32, record.ID);
                DataAccess.AddInParameter(cmd, "@pAccountID", DbType.Int32, record.AccountID);
                DataAccess.AddInParameter(cmd, "@pRecordType", DbType.Boolean, record.RecordType);
                DataAccess.AddInParameter(cmd, "@pRecordCategoryID", DbType.Int32, record.CategoryID);
                DataAccess.AddInParameter(cmd, "@pAmount", DbType.Decimal, record.Amount);
                DataAccess.AddInParameter(cmd, "@pDateTimeOffset", DbType.DateTimeOffset, record.DateTimeOffset);
                DataAccess.AddInParameter(cmd, "@pComment", DbType.String, record.Comment);
                var obj = DataAccess.ExecuteScalar(cmd);
                return Convert.ToInt32(obj);
            }
        }
    }
}
