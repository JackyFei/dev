using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebApiApplication.Models
{
    [DataContract(Name ="product")]
    public class Product
    {
        [Required]
        [MaxLength(10)]
        [DataMember(Name = "id", Order = 1)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Invalid product")]
        [MaxLength(30)]
        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }

        [Required]
        [Range(0.00, 999999.99, ErrorMessage = "Invalid price")]
        [RegularExpression(@"^\d+\.\d{1,2}$", ErrorMessage = "Invalid price!!")]
        [DataMember(Name = "price", Order = 3)]
        public decimal Price { get; set; }
    }
}