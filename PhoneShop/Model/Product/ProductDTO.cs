using PhoneShop.Data;

namespace PhoneShop.Model.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public string Category { get; set; }
        public ICollection<ImageProductDTO> ImageProducts { get; set; }
    }
}
