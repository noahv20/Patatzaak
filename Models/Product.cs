using System.ComponentModel.DataAnnotations;
using Patatzaak.Controllers;

namespace Patatzaak.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int Sale {  get; set; }
        [Required]
        public int Stock { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
