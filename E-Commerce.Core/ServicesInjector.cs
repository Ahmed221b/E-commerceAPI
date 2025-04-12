using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Core
{
    public static class ServicesInjector
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService,CategoryService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, StripePaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IWishlistService, WishlistService>();
            return services;
        }
    }
}
