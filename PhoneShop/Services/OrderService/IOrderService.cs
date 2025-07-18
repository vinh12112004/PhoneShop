using PhoneShop.Model.Order;

namespace PhoneShop.Services.OrderService
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetOrder(int userId);
        Task <bool> CreateOrder(int userId, CreateOrderDTO dto);
        Task <bool> UpdateOrder(int userId, CreateOrderDTO dto);
    }
}
