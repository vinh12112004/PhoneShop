using PhoneShop.Model;

namespace PhoneShop.Services
{
    public interface ICartService
    {
        Task<CartDTO> GetCartAsync(int userId);
        Task <bool> AddToCartAsync(int userId, AddToCartDTO dto);
        Task <bool> RemoveFromCartAsync(int userId, AddToCartDTO dto);
        Task <bool> ClearCartAsync(int userId);
    }
}
