
using API.Filters;
using E_Commerce.Core;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Services;
using E_Commerce.Data;
using E_Commerce.EF;
using E_Commerce.EF.Repositories;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_Commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddDbContext<ApplicationDBContext>(
                options =>  
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .UseLazyLoadingProxies();
                }
            );
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddControllers(
                options =>
                {
                    // Register the filter globally
                    options.Filters.Add<ValidateModelAttribute>();
                }
              );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Injecting Services and Repositories.
            builder.Services.AddRepositories();
            builder.Services.AddServices();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
