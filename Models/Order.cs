using System.ComponentModel.DataAnnotations;

namespace Patatzaak.Models
{
    public class Order
    {
        [Key]
        public int OrderNr { get; set; }
        [Required]
        public DateTime OrderedOn { get; set; } = DateTime.Now;
        [Required]
        public string State { get; set; } // Value's 'Not started yet', 'In progress' and 'Done'
        public List<OrderItem>? OrderItems { get; set; }
        public Customer? Customer { get; set; }
        public int? CustomerId { get; set; }
    }
}
