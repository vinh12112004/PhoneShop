using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model.Order;

namespace PhoneShop.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IPhoneShopRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IPhoneShopRepository<Order> cartRepository, IMapper mapper)
        {
            _orderRepository = cartRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateOrder(int userId, CreateOrderDTO dto)
        {
            Order order = _mapper.Map<Order>(dto);
            order.UserId = userId;
            order.CreatedAt = DateTime.UtcNow;
            await _orderRepository.CreateAsync(order);

            return true;
        }
        public async Task<List<OrderDTO>> GetOrder(int userId)
        {
            var orders = await _orderRepository.GetAllAsyncByFilter(c => c.UserId == userId, c => c.Include(c => c.OrderProducts).ThenInclude(ci => ci.Product));
            if (orders == null)
            {
                throw new ArgumentException($"Order for user with id {userId} not found.");
            }
            List<OrderDTO> OrderDTOs = _mapper.Map<List<OrderDTO>>(orders);
            return OrderDTOs;
        }

        public Task<bool> UpdateOrder(int userId, CreateOrderDTO dto)
        {
            throw new NotImplementedException();
        }


        //public async Task<bool> UpdateOrder(int userId, CreateOrderDTO dto)
        //{
        //    var cart = await _orderRepository.GetAsync(c => c.UserId == userId, c => c.Include(c => c.OrderProducts).ThenInclude(ci => ci.Product));
        //    if (cart == null)
        //    {
        //        throw new ArgumentException($"Cart for user with id {userId} not found.");
        //    }
        //    var existingItem = cart.OrderProducts.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
        //    if (existingItem == null)
        //    {
        //        throw new ArgumentException($"Product with id {dto.ProductId} not found in cart for user with id {userId}.");
        //    }
        //    if(existingItem.Quantity > dto.Quantity)
        //    {
        //        existingItem.Quantity -= dto.Quantity;
        //        await _orderRepository.UpdateAsync(cart);
        //        return true;
        //    }
        //    else
        //    {
        //        cart.OrderProducts.Remove(existingItem);
        //        await _orderRepository.UpdateAsync(cart);
        //    }

        //    return true;
        //}
    }
}
