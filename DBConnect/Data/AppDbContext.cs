using DBConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace DBConnect.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Product> Products { get; set; }

        // Added entities
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                        .Property(p => p.Price)
                        .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                        .Property(o => o.Total)
                        .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetail>()
                        .Property(od => od.UnitPrice)
                        .HasColumnType("decimal(18,2)");
        }
    }
}
