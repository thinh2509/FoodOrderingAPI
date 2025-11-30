using FoodOrdering.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrdering.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fluent API
            // Xóa Category thì xóa luôn MenuItems
            builder.Entity<Category>()
                .HasMany(c => c.MenuItems)
                .WithOne(m => m.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            // Xóa Restaurant thì xóa luôn Categories
            builder.Entity<Restaurant>()
                .HasMany(c => c.Categories)
                .WithOne(m => m.Restaurant)
                .HasForeignKey(m => m.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
            // Khi xóa MenuItem -> KHÔNG ĐƯỢC xóa OrderDetail (Lịch sử đơn hàng là bất tử)
            builder.Entity<OrderDetail>()
                .HasOne(od => od.MenuItem)
                .WithMany()
                .HasForeignKey(od => od.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
            // Tương tự, ta nên chặn xóa Restaurant nếu đã có Order
            builder.Entity<Order>()
                .HasOne(o => o.Restaurant)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình cho thuộc tính price
            // Để cấu hình cho thuộc tính mà có ở nhiều entity mà không cần cấu hình từng cái ta sử dụng foreach
            // Lặp qua tất cả các property có kiểu decimal để set precision
            // 1. Duyệt qua tất cả Entity được đăng ký trong DbContext
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // Lấy ra các property trong entity kèm theo có kiểu là decimal
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal));
                // Duyệt qua từng property và set trường
                foreach(var property in properties)
                {
                    builder.Entity(entityType.Name).Property(property.Name)
                        .HasColumnType("decimal(18, 2)");
                }
            }
        }
    }
}
