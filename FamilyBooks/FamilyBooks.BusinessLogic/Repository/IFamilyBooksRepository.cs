using FamilyBooks.BusinessLogic.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyBooks.BusinessLogic.Repository
{
    public interface IFamilyBooksRepository
    {
        int CreateRecord(RecordBase record);
    }
}
