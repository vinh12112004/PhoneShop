using PhoneShop.Data;

namespace PhoneShop.Model
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public double TotalPrice
        {
            get
            {
                return CartItems?.Sum(item => item.Product.Price * item.Quantity) ?? 0;
            }
        }
    }
}
