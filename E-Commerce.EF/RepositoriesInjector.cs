using E_Commerce.Core.Interfaces;
using E_Commerce.Core;
using E_Commerce.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;
using E_Commerce.Core.Interfaces.Repositories;

namespace E_Commerce.EF
{
    public static class RepositoriesInjector
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
