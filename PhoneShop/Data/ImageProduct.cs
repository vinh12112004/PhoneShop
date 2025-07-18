using System.Text.Json.Serialization;

namespace PhoneShop.Data
{
    public class ImageProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
