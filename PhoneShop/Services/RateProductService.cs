using AutoMapper;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace PhoneShop.Services
{
    public class RateProductService : IRateProductServicce
    {
        private readonly IPhoneShopRepository<RateProduct> _rateProductRepository;
        private readonly IPhoneShopRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        public RateProductService(IPhoneShopRepository<RateProduct> rateProductRepository, IMapper mapper, IPhoneShopRepository<Product> productRepository)
        {
            _rateProductRepository = rateProductRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<bool> AddOrUpdateRateProductAsync(int UserId, RateProductDTO dto)
        {
            RateProduct newRecord = _mapper.Map<RateProduct>(dto);
            newRecord.UserId = UserId;
            var rateProduct = await _rateProductRepository.GetAsync(rp => rp.ProductId == dto.ProductId && rp.UserId == UserId, true);
            if (rateProduct == null)
            {
                rateProduct = await _rateProductRepository.CreateAsync(newRecord);
            }
            else
            {
                rateProduct.Rate = dto.Rate;
                await _rateProductRepository.UpdateAsync(rateProduct);
            }
            await setAVGRate(dto.ProductId);
            return true;
        }

        public async Task<List<RateProductDTO>> GetRateProduct()
        {
            var rateProducts = await _rateProductRepository.GetAllAsync();
            var rateProductDTOs = _mapper.Map<List<RateProductDTO>>(rateProducts);
            return rateProductDTOs;
        }

        public async Task<RateProductDTO> GetRateProductByIdAsync(int id)
        {
            RateProduct rateProduct = await _rateProductRepository.GetAsync(rp => rp.Id == id, true);
            RateProductDTO rateProductDTO = _mapper.Map<RateProductDTO>(rateProduct);
            return rateProductDTO;
        }

        public async Task<bool> RemoveRateProductAsync(int UserId, int ProductId)
        {
            var rateProduct = await _rateProductRepository.GetAsync(rp => rp.ProductId == ProductId && rp.UserId == UserId, true);
            if (rateProduct == null)
            {
                return false; // RateProduct not found
            }
            await _rateProductRepository.DeleteAsync(rateProduct);
            return true;
        }
        public async Task setAVGRate(int productId)
        {
            var rateProducts = await _rateProductRepository.GetAllAsyncByFilter(rp => rp.ProductId == productId);
            double avgRate = rateProducts.Average(rp => rp.Rate);
            var product = await _productRepository.GetAsync(p => p.Id == productId );

            product.Rate = avgRate;
            await _productRepository.UpdateAsync(product);
        }
    }
}
