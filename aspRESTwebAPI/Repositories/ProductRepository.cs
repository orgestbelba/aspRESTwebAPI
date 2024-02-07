using aspRESTwebAPI.Data;
using aspRESTwebAPI.Interfaces;
using aspRESTwebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace aspRESTwebAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public ICollection<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProduct(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
        public bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.ProductId == productId);
        }

        //Methods regarding the Many-To-Many relationship
        public ICollection<Customer> GetCustomersForProduct(int productId)
        {
            var customers = _context.CustomerProducts
                .Where(cp => cp.ProductId == productId)
                .Select(cp => cp.Customer)
                .ToList();

            return customers;
        }

        public void AddCustomerToProduct(int productId, int customerId)
        {
            var customerProduct = new CustomerProduct { ProductId = productId, CustomerId = customerId };
            _context.CustomerProducts.Add(customerProduct);
            _context.SaveChanges();
        }

        public void RemoveCustomerFromProduct(int productId, int customerId)
        {
            var customerProduct = _context.CustomerProducts
                .FirstOrDefault(cp => cp.ProductId == productId && cp.CustomerId == customerId);

            if (customerProduct != null)
            {
                _context.CustomerProducts.Remove(customerProduct);
                _context.SaveChanges();
            }
        }

    }
}
