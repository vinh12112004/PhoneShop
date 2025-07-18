namespace PhoneShop.Model.Order
{
    public class CreateOrderDTO
    {
        public string? ShippingAddress { get; set; }
        public List<OrderProductDTO> OrderProducts { get; set; }
    }
}
