﻿using E_Commerce.Core.Models;
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
            modelBuilder.Entity<Product>()
            .ToTable(tb => tb.HasTrigger("trg_UpdateCartOnPriceChange"));
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

            //One To One (Customer - Cart)
            modelBuilder.Entity<Customer>()
                .HasOne(p => p.Cart)
                .WithOne(p => p.Customer)
                .HasForeignKey<Cart>(p => p.CustomerId);

            //One To One (Customer - Wishlist)
            modelBuilder.Entity<Customer>()
               .HasOne(p => p.Wishlist)
               .WithOne(p => p.Customer)
               .HasForeignKey<Wishlist>(p => p.CustomerId);

            //Many To Many (Cart - Product)
            modelBuilder.Entity<CartItem>()
                .HasKey(p => new {p.CartId, p.ProductId});

            modelBuilder.Entity<CartItem>()
                .HasOne(p => p.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<CartItem>()
                .HasOne(p => p.Cart)
                .WithMany(p => p.CartItems)
                .HasForeignKey(p => p.CartId);

            //Many To Many (Wishlist - Product)
            modelBuilder.Entity<WishlistItem>()
                .HasKey(p => new { p.WishlistId, p.ProductId });

            modelBuilder.Entity<WishlistItem>()
                .HasOne(p => p.Product)
                .WithMany(p => p.WishlistItems)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<WishlistItem>()
                .HasOne(p => p.Wishlist)
                .WithMany(p => p.WishlistItems)
                .HasForeignKey(p => p.WishlistId);

            //One To Many (ApplicationUser - RefreshTokens)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.RefreshTokens)
                .WithOne(p => p.ApplicationUser)
                .HasForeignKey(p => p.ApplicationUserId);

            //One To Many (Customer - Payment)
            modelBuilder.Entity<Customer>()
                .HasMany(p => p.Payments)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId);

            //One To One (Order - Payment)
            modelBuilder.Entity<Order>()
                .HasOne(p => p.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.NoAction);




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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Payment> Payments { get; set; }

    }
}
