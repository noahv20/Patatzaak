using System.ComponentModel.DataAnnotations;

namespace Patatzaak.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Phone {  get; set; }
        public int? SavedPoints { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
