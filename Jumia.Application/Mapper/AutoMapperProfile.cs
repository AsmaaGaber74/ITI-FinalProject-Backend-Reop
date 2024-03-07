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
            CreateMap<RegisterViewModel,ApplicationUser>().ReverseMap();
            CreateMap<LoginViewModel,ApplicationUser>().ReverseMap();
            
            CreateMap<RoleViewModel,ApplicationUser>().ReverseMap();
         
        }
    }
}
