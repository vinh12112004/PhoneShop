using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model;

namespace PhoneShop.Services
{
    public class CartService : ICartService
    {
        private readonly IPhoneShopRepository<Cart> _cartRepository;
        private readonly IMapper _mapper;

        public CartService(IPhoneShopRepository<Cart> cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddToCartAsync(int userId, AddToCartDTO dto)
        {
            var cart = await _cartRepository.GetAsync(c => c.UserId == userId, c => c.Include(c=> c.CartItems),true);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                await _cartRepository.CreateAsync(cart);
            }
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            if (existingItem == null)
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    CartId = cart.Id
                });
            }
            else
            {
                existingItem.Quantity += dto.Quantity;
            }
            await _cartRepository.UpdateAsync(cart);

            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetAsync(c => c.UserId == userId, c => c.Include(c => c.CartItems));
            if (cart == null)
            {
                throw new ArgumentException($"Cart for user with id {userId} not found.");
            }
            await _cartRepository.DeleteAsync(cart);
            return true;
        }

        public async Task<CartDTO> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetAsync(c => c.UserId == userId, c => c.Include(c => c.CartItems).ThenInclude(ci => ci.Product));
            if (cart == null)
            {
                throw new ArgumentException($"Cart for user with id {userId} not found.");
            }
            CartDTO cartDTO = _mapper.Map<CartDTO>(cart);
            return cartDTO;
        }

        public async Task<bool> RemoveFromCartAsync(int userId, AddToCartDTO dto)
        {
            var cart = await _cartRepository.GetAsync(c => c.UserId == userId, c => c.Include(c => c.CartItems).ThenInclude(ci => ci.Product));
            if (cart == null)
            {
                throw new ArgumentException($"Cart for user with id {userId} not found.");
            }
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            if (existingItem == null)
            {
                throw new ArgumentException($"Product with id {dto.ProductId} not found in cart for user with id {userId}.");
            }
            if(existingItem.Quantity > dto.Quantity)
            {
                existingItem.Quantity -= dto.Quantity;
                await _cartRepository.UpdateAsync(cart);
                return true;
            }
            else
            {
                cart.CartItems.Remove(existingItem);
                await _cartRepository.UpdateAsync(cart);
            }
                
            return true;
        }
    }
}
