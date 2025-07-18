using PhoneShop.Model;

namespace PhoneShop.Services.RateProductService
{
    public interface IRateProductServicce
    {
        // get all rate products
        Task<List<RateProductDTO>> GetRateProduct();
        // get rate product by userId and productId
        Task<RateProductDTO> GetRateProductByIdAsync(int userId, int id);
        // add or update rate product
        Task<bool> AddOrUpdateRateProductAsync(int UserId, RateProductDTO dto);
        Task<bool> RemoveRateProductAsync(int UserId, int ProductId);
    }
}
