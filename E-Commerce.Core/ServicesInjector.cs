using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using ProductService = E_Commerce.Core.Services.ProductService;
using ReviewService = E_Commerce.Core.Services.ReviewService;

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
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IPaymentService, StripePaymentService>();


            return services;
        }
    }
}
