using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.DTO.Cart;
using E_Commerce.Core.DTO.Category;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.DTO.Order;
using E_Commerce.Core.DTO.Product;
using E_Commerce.Core.DTO.Role;
using E_Commerce.Core.DTO.Wishlist;
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

            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => Convert.ToBase64String(src.Product.Image)))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPriceOfItems))
                .ReverseMap();


            CreateMap<Cart, CartItemsDTO>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
                .ReverseMap();

            CreateMap<Order, GetOrderDTO>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderProducts.Select(op => new OrderItemDTO
                {
                    Id = op.OrderId,
                    ProductName = op.Product.Name,
                    Price = op.Price,
                    Quantity = op.Quantity
                }).ToList())).ReverseMap();

            CreateMap<Wishlist,GetWishlistDTO>()
                .ForMember(dest => dest.WishlistId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WishlistItems, opt => opt.MapFrom(src => src.WishlistItems.Select(wi => new WishlistItemDTO
                {
                    ProductId = wi.ProductId,
                    ProductName = wi.Product.Name,
                    ProductImageBase64 = Convert.ToBase64String(wi.Product.Image),
                    ProductPrice = wi.Product.Price

                }).ToList()))
                .ReverseMap();


        }
    }
    
    
}
