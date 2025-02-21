using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.DTO.Category;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.DTO.Product;
using E_Commerce.Core.DTO.Role;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, CategpryProductDTO>().ReverseMap();
            CreateMap<Category,GetCategoryDTO>().ReverseMap();
            CreateMap<Category,GetCategoryListDTO>().ReverseMap();
            CreateMap<Color, ColorDTO>().ReverseMap();



            CreateMap<AddProductDTO, Product>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.ImageBase64)))
                .ReverseMap();

            CreateMap<Product, GetProductDTO>()
                .ForMember(dest => dest.ImageBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image))) // Convert byte array to Base64 string
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.ProductColors
                    .Where(pc => pc.Color != null)
                    .Select(pc => pc.Color.Name)
                    .ToList()));

            CreateMap<ApplicationUser, UsersInRoleDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<IdentityRole,GetRoleDTO>()
                .ForMember(dest => dest.RoleName,opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
    
    
}
