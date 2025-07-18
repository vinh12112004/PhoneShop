using Microsoft.EntityFrameworkCore;
using PhoneShop.Data.Config;

namespace PhoneShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Carts { get; set; }
        public DbSet<OrderProduct> CartItems { get; set; }
        public DbSet<RateProduct> RateProducts { get; set; }
        public DbSet<ImageProduct> ImageProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new OrderProductConfig());
            modelBuilder.ApplyConfiguration(new RateProductConfig());
            modelBuilder.ApplyConfiguration(new ImageProductConfig());
        }
    }
}
