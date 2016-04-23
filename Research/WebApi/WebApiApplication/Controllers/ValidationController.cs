using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Http;

namespace WebApiApplication.Controllers
{
    [RoutePrefix("validations")]
    public class ValidationController : ApiController
    {
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {              
                return BadRequest(ModelState);
            }
            var str = "05.55";
            var price = decimal.Parse(str);
            return Ok(price);
        }
    }

    [DataContract]
    public class Product
    {
        [Required(ErrorMessage = "Invalid product")]
        [DataMember(Name = "product", Order = 1)]
        public string Name { get; set; }

        [Range(0.00, int.MaxValue, ErrorMessage = "Invalid price")]
        [RegularExpression(@"^\d+\.\d{1,2}$", ErrorMessage ="Invalid price!!")]
        [DataMember(Name = "price", Order = 2)]
        public string Price { get; set; }
    }
}
