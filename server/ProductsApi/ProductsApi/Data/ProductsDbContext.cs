﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Data
{
    public class ProductsDbContext : IdentityDbContext<AppUser>
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().Property(x => x.Status).HasDefaultValue(Status.New);

            modelBuilder.Entity<Product>()
                .HasOne(a => a.Category)
                .WithMany(b => b.Products)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrderItem>()
                .HasOne(a => a.Order)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(a => a.OrderId);

            modelBuilder.Entity<OrderItem>()
               .HasOne(a => a.Product)
               .WithMany()
               .HasForeignKey(a => a.ProductId);

            modelBuilder.Entity<Order>()
              .HasOne(a => a.AppUser)
              .WithMany(b => b.Orders)
              .HasForeignKey(a => a.AppUserId);
        }
    }
}
