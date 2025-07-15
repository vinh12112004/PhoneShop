using AutoMapper;
using PhoneShop.Data;
using PhoneShop.Model;

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
            CreateMap<Cart, CartDTO>();
            CreateMap<RateProductDTO, RateProduct>().ReverseMap();
            //CreateMap<CartItem, CartItemDTO>()
            //    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));
            // Add other mappings here as needed
        }
    }
}
