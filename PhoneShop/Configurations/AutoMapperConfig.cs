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
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserReadonyDTO>().ReverseMap();
            // Add other mappings here as needed
        }
    }
}
