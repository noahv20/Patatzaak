using System.ComponentModel.DataAnnotations;

namespace Patatzaak.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfEmployment { get; set; } = DateTime.Now;
    }
}
