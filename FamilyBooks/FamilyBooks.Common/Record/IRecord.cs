namespace FamilyBooks.Common.Record
{
    public interface IFamilyBooksFacade
    {
        #region Expenditure
        int RecordExpenditure(Expenditure expenditure);
        void UpdateExpenditure(Expenditure expenditure);
        void DeleteExpenditure(Expenditure expenditure);
        #endregion
        #region Income
        int RecordIncome(Income income);
        void UpdateIncome(Income income);
        void DeleteIncome(Income income);
        #endregion
    }
}