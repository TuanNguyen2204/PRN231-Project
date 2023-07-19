using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using System.Data;

namespace eClothesAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Color, ColorDTO>();
            CreateMap<Size, SizeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName));
            CreateMap<ProductCreateUpdateDTO, Product>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
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
            CreateMap<Order, OrderDTO>().ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.User.Username));
            CreateMap<OrderCreateUpdateDTO, Order>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>()
                 .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.SizeName,
                opt => opt.MapFrom(src => src.Size.SizeName))
                 .ForMember(dest => dest.ColorName,
                opt => opt.MapFrom(src => src.Color.ColorName));
        }
    }
}
