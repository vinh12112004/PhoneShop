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
            // Add other mappings here as needed
        }
    }
}
