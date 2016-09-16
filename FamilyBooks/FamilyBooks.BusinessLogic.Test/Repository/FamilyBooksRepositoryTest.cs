using System;
using FamilyBooks.BusinessLogic.Record;
using FamilyBooks.BusinessLogic.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyBooks.BusinessLogic.Test.Repository
{
    [TestClass]
    public class FamilyBooksRepositoryTest
    {
        private readonly IFamilyBooksRepository _familyBooksRepository = new FamilyBooksRepository();

        [TestMethod]
        public void CreateRecord_Success()
        {
            var expenditure = new Expenditure
            {
                AccountID = 1,
                CategoryID = 1,
                Amount = Convert.ToDecimal(new Random().NextDouble()),
                DateTimeOffset = DateTimeOffset.Now,
                Comment = $"[{new Random().Next()}]Test CreateRecord."
            };

            var id = _familyBooksRepository.CreateRecord(expenditure);
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void UpdateRecord_Success()
        {
            var expenditure = new Expenditure
            {
                AccountID = 1,
                CategoryID = 1,
                Amount = Convert.ToDecimal(new Random().NextDouble()),
                DateTimeOffset = DateTimeOffset.Now,
                Comment = $"[{new Random().Next()}]Test CreateRecord."
            };

            var id = _familyBooksRepository.CreateRecord(expenditure);
            Assert.IsTrue(id > 0);

            expenditure.ID = id;
            expenditure.Comment = $"[{new Random().Next()}]Test UpdateRecord.";
            var updatedCount = _familyBooksRepository.UpdateRecord(expenditure);
            Assert.IsTrue(updatedCount == 1);
        }
    }
}
