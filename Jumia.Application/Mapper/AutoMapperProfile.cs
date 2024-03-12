using AutoMapper;
using Jumia.Dtos;
using Jumia.Dtos.ViewModel;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Jumia.Application.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<CategoryDto,Category >().ReverseMap();

            CreateMap<CreateOrUpdateProuductDTO, Product>().ReverseMap();
            CreateMap<GetAllProuductDTO, Product>().ReverseMap();
            CreateMap<ProuductViewModel, Product>().ReverseMap()
             .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();

            CreateMap<CateogaryViewModel, Category>().ReverseMap();
            CreateMap<RegisterViewModel,ApplicationUser>().ReverseMap();
            CreateMap<LoginViewModel,ApplicationUser>().ReverseMap();
            CreateMap<RoleViewModel,ApplicationUser>().ReverseMap();
            CreateMap<ItemViewModel,Item>().ReverseMap();
            CreateMap<OrderDto,Order>().ReverseMap();
            CreateMap<ProductImageDto,ProductImage>().ReverseMap();
            CreateMap<ItemViewModel, Item>()
                            .ForMember(dest => dest.ItemImage, opt => opt.MapFrom(src => src.ItemImagestring))
                            .ReverseMap()
                            .ForMember(dest => dest.ItemImage, opt => opt.Ignore())
                            .ForMember(dest => dest.ItemImagestring, opt => opt.MapFrom(src => src.ItemImage));
            CreateMap<UserViewModel, ApplicationUser>().ReverseMap();
        }

    }
}
