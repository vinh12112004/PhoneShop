namespace PhoneShop.Model.Product
{
    public class ProductFilterDTO
    {
        public double priceFrom { get; set; }
        public double priceTo { get; set; }
        public double rateFrom { get; set; }
        public double rateTo { get; set; }
        public string category { get; set; }
    }
}
