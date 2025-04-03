using Assignment_2.Models;

namespace Assignment_2.ViewModels
{
    public class OrderOderItemProductViewModel
    {
        public Order Order { get; set; }
        public List<OrderItem> orderItems { get; set; }
        public List<Product> products { get; set; }
    }
}
