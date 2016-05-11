using System.Web.Http;
using WebApiApplication.Models;

namespace WebApiApplication.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : ApiController
    {
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Product product)
        {
            if(product == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", product);
        }
    }
}
