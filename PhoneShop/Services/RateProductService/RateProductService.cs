using AutoMapper;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace PhoneShop.Services.RateProductService
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
        // add or update rate product

        public async Task<bool> AddOrUpdateRateProductAsync(int UserId, RateProductDTO dto)
        {
            // map dto to RateProduct entity
            RateProduct newRecord = _mapper.Map<RateProduct>(dto);
            newRecord.UserId = UserId;
            // check if the rate product already exists for the user and product
            var rateProduct = await _rateProductRepository.GetAsync(rp => rp.ProductId == dto.ProductId && rp.UserId == UserId, true);
            // if it exists, update it; otherwise, create a new one
            if (rateProduct == null)
            {
                rateProduct = await _rateProductRepository.CreateAsync(newRecord);
            }
            else
            {
                rateProduct.Rate = dto.Rate;
                await _rateProductRepository.UpdateAsync(rateProduct);
            }
            // set average rate for the product
            await setAVGRate(dto.ProductId);
            return true;
        }
        // get all rate products
        public async Task<List<RateProductDTO>> GetRateProduct()
        {
            var rateProducts = await _rateProductRepository.GetAllAsync();
            var rateProductDTOs = _mapper.Map<List<RateProductDTO>>(rateProducts);
            return rateProductDTOs;
        }
        // get rate product by userId and productId
        public async Task<RateProductDTO> GetRateProductByIdAsync(int userId, int id)
        {
            var rateProduct = await _rateProductRepository.GetAsync(rp => rp.ProductId == id && rp.UserId == userId, true);
            RateProductDTO rateProductDTO = _mapper.Map<RateProductDTO>(rateProduct);
            return rateProductDTO;
        }
        // remove rate product by userId and productId
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
        // set average rate for product
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
