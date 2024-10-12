using Patatzaak.Models;
namespace Patatzaak.ViewModels
{
    public class ProductOrder
    {
        public List<Product>? Products { get; set; }
        public Order Order { get; set; }
        public List<OrderItem> Items { get; set; }
        public List<Product>? ProductsInOrder { get; set; }
    }
}
