using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;

namespace eClothesAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Color, ColorDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName));
            CreateMap<ProductCreateUpdateDTO, Product>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Inventory, InventoryDTO>()
                .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.SizeName,
                opt => opt.MapFrom(src => src.Size.SizeName))
                 .ForMember(dest => dest.ColorName,
                opt => opt.MapFrom(src => src.Color.ColorName))
                 .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Product.Category.CategoryName));
            CreateMap<InventoryCreateUpdateDTO, Inventory>().ReverseMap();

        }
    }
}
