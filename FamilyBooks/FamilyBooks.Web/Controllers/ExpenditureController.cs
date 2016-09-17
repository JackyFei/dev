using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyBooks.Common.Record;

namespace FamilyBooks.Web.Controllers
{
    public class ExpenditureController : Controller
    {
        [HttpGet]
        [ActionName("CreateExpenditure")]
        public ActionResult CreateExpenditure()
        {
            return View();
        }

        [HttpPost]
        [ActionName("CreateExpenditure")]
        public ActionResult CreateExpenditure(Expenditure expenditure)
        {
            var result = new ContentResult {Content = expenditure.Comment, ContentType = "application/text"};
            return result;
        }
    }
}