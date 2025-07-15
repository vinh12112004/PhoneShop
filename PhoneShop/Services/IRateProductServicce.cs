using PhoneShop.Model;

namespace PhoneShop.Services
{
    public interface IRateProductServicce
    {
        Task<List<RateProductDTO>> GetRateProduct();
        Task<RateProductDTO> GetRateProductByIdAsync(int id);
        Task<bool> AddOrUpdateRateProductAsync(int UserId, RateProductDTO dto);
        Task<bool> RemoveRateProductAsync(int UserId, int ProductId);
    }
}
