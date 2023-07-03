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
        }
    }
}
