using aspRESTwebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace aspRESTwebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerProduct> CustomerProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-To-Many Relationship
            modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);

            // Many-To-Many Relationship
            modelBuilder.Entity<CustomerProduct>()
            .HasKey(cp => new { cp.CustomerId, cp.ProductId });

            modelBuilder.Entity<CustomerProduct>()
            .HasOne(cp => cp.Customer)
            .WithMany(c => c.CustomerProducts)
            .HasForeignKey(cp => cp.CustomerId);

            modelBuilder.Entity<CustomerProduct>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CustomerProducts)
            .HasForeignKey(cp => cp.ProductId);

            base.OnModelCreating(modelBuilder);

        }
    }
}
