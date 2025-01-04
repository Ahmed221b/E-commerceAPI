using E_Commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext() 
        {
        }
        
        public ApplicationDBContext (DbContextOptions options) : base(options) 
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Customer>().ToTable("Customers");

            //One To Many (Customer - Order)
            modelBuilder.Entity<Customer>()
                .HasMany(p => p.Orders)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId);

            //One To Many (Category - Product)
            modelBuilder.Entity<Category>()
                .HasMany(p => p.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            //Many To Many Review relationship (Customer - Product)
            modelBuilder.Entity<CustomerReview>()
                .HasKey(p => new { p.CustomerId, p.ProductId });

            modelBuilder.Entity<CustomerReview>()
                .HasOne(p => p.Customer)
                .WithMany(p => p.CustomerReviews)
                .HasForeignKey(p => p.CustomerId);

            modelBuilder.Entity<CustomerReview>()
               .HasOne(p => p.Product)
               .WithMany(p => p.CustomerReviews)
               .HasForeignKey(p => p.ProductId);


            //Many To Many (Product - Order)
            modelBuilder.Entity<OrderProduct>()
                .HasKey(p => new {p.OrderId, p.ProductId});

            modelBuilder.Entity<OrderProduct>()
                .HasOne(p => p.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(p => p.Order)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(p => p.OrderId);

            //Many To Many (Product - Color)
            modelBuilder.Entity<ProductColor>()
                .HasKey(p => new { p.ProductId, p.ColorId });

            modelBuilder.Entity<ProductColor>()
                .HasOne(p => p.Product)
                .WithMany(p => p.ProductColors)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<ProductColor>()
               .HasOne(p => p.Color)
               .WithMany(p => p.ProductColors)
               .HasForeignKey(p => p.ColorId);



        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }
    }
}
