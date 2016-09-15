using System;
using FamilyBooks.Common.Record;

namespace FamilyBooks.Facade
{
    public class FamilyBooksFacade : IFamilyBooksFacade
    {
        private readonly Converter _converter = new Converter();
        private readonly BusinessLogic.Record.RecordManager _recordManager = new BusinessLogic.Record.RecordManager();

        public int RecordExpenditure(Expenditure expenditure)
        {          
            try
            {
                var modelExpenditure = _converter.Convert(expenditure);
                return _recordManager.RecordExpenditure(modelExpenditure);
            }
            catch (BusinessLogic.Exceptions.ValidationException ex)
            {
                return 0;
            }       
        }

        public void UpdateExpenditure(Expenditure expenditure)
        {
            throw new NotImplementedException();
        }

        public void DeleteExpenditure(Expenditure expenditure)
        {
            throw new NotImplementedException();
        }

        public int RecordIncome(Income income)
        {
            throw new NotImplementedException();
        }

        public void UpdateIncome(Income income)
        {
            throw new NotImplementedException();
        }

        public void DeleteIncome(Income income)
        {
            throw new NotImplementedException();
        }
    }
}