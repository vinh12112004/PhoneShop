using PhoneShop.Data;

namespace PhoneShop.Model.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public double TotalPrice
        {
            get
            {
                return OrderProducts?.Sum(item => item.Product.Price * item.Quantity) ?? 0;
            }
        }
    }
}
