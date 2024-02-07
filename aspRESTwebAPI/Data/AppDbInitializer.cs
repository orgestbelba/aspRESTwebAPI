using aspRESTwebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace aspRESTwebAPI.Data
{
    public static class AppDbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate(); // Ensure the database is created and migrated

            // Check if there's already data.
            if (context.Customers.Any() || context.Products.Any() || context.Orders.Any() || context.CustomerProducts.Any())
            {
                return; // No need for data to be seeded
            }

            SeedData(context);
        }

        private static void SeedData(AppDbContext context)
        {
            // Seed Customers
            var customers = new[]
            {
            new Customer { FirstName = "Orgest", LastName = "Belba", Email = "orgestbelba@gmail.com" },
            new Customer { FirstName = "Filan", LastName = "Fisteku", Email = "filan.fisteku@outlook.com" },
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();

            // Seed Products
            var products = new[]
            {
            new Product { Name = "Product 1", Price = 19.99 },
            new Product { Name = "Product 2", Price = 29.50 },
            new Product { Name = "Product 3", Price = 9.99 },
            new Product { Name = "Product 4", Price = 13 },
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            // Seed Orders
            var orders = new[]
            {
            new Order { CustomerId = 1, OrderDate = DateTime.Now, TotalAmount = 39.05 },
            new Order { CustomerId = 2, OrderDate = DateTime.Now, TotalAmount = 59.98 },
            new Order { CustomerId = 1, OrderDate = DateTime.Now, TotalAmount = 29.50 },
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();

            // Seed CustomerProducts (Many-to-Many relationship)
            var customerProducts = new[]
            {
            new CustomerProduct { CustomerId = 1, ProductId = 1 },
            new CustomerProduct { CustomerId = 2, ProductId = 2 },
            new CustomerProduct { CustomerId = 1, ProductId = 2 },
            new CustomerProduct { CustomerId = 2, ProductId = 1 },
            };
            context.CustomerProducts.AddRange(customerProducts);
            context.SaveChanges();
        }
    }
}
