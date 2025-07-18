using AutoMapper;
using PhoneShop.Data;
using PhoneShop.Model;
using PhoneShop.Model.Order;
using PhoneShop.Model.Product;
using PhoneShop.Model.User;

namespace PhoneShop.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<User, UserReadonyDTO>().ReverseMap();
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();

            //map de tao order
            CreateMap<CreateOrderDTO, Order>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Set riêng trong service
               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Set riêng trong service
               .ForMember(dest => dest.User, opt => opt.Ignore())
               .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<OrderProductDTO, OrderProduct>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            //map de tao rate product
            CreateMap<RateProductDTO, RateProduct>().ReverseMap();
            CreateMap<ImageProduct, ImageProductDTO>().ReverseMap();

        }
    }
}
