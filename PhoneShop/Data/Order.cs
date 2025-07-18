namespace PhoneShop.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
