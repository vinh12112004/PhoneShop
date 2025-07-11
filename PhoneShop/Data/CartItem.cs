using System.Text.Json.Serialization;

namespace PhoneShop.Data
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        // Navigation properties
        [JsonIgnore]
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
