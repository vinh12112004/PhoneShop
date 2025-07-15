namespace PhoneShop.Data
{
    public class RateProduct
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public double Rate { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
