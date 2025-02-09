using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.DTO.Category;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Models;

namespace E_Commerce.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category,GetCategoryDTO>().ReverseMap();
            CreateMap<Category,GetCategoryListDTO>().ReverseMap();
            CreateMap<Color, GetColorDTO>().ReverseMap();
        }
    }
}
