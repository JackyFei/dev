using System.Web.Http;

namespace FamilyBooks.Webapi.Controllers
{
    [RoutePrefix("expenditure")]
    public class ExpenditureController : ApiController
    {
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateExpenditure()
        {
            return Ok("Create");
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateExpenditure()
        {
            return Ok("Update");
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetExpenditureDetails(string id)
        {
            return Ok("Get");
        }
    }
}
