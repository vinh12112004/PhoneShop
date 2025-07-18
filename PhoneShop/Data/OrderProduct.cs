using System.Text.Json.Serialization;

namespace PhoneShop.Data
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        // Navigation properties
        [JsonIgnore]
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
