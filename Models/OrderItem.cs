using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Patatzaak.Models
{
    public class OrderItem
    {
        
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        
        public int OrderNr { get; set; }
        public Order? Order { get; set; }
        [Required]
        public int Amount {  get; set; }
    }
}
