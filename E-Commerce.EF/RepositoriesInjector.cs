using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Services;
using E_Commerce.Core;
using E_Commerce.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;

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
