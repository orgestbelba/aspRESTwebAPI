using aspRESTwebAPI.Data;
using aspRESTwebAPI.Interfaces;
using aspRESTwebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace aspRESTwebAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public ICollection<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }
        public Customer GetCustomer(int customerId)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == customerId);
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public bool CustomerExists(int customerId)
        {
            return _context.Customers.Any(c => c.Id == customerId);
        }

        //Methods regarding the One-To-Many relationship
        public ICollection<Order> GetOrdersForCustomer(int customerId)
        {
            var customer = _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefault(c => c.Id == customerId);

            return customer?.Orders;
        }

        //Methods regarding the Many-To-Many relationship
        public ICollection<Product> GetProductsForCustomer(int customerId)
        {
            var products = _context.CustomerProducts
                .Where(cp => cp.CustomerId == customerId)
                .Select(cp => cp.Product)
                .ToList();

            return products;
        }

        public void AddProductToCustomer(int customerId, int productId)
        {
            var customerProduct = new CustomerProduct { CustomerId = customerId, ProductId = productId };
            _context.CustomerProducts.Add(customerProduct);
            _context.SaveChanges();
        }

        public void RemoveProductFromCustomer(int customerId, int productId)
        {
            var customerProduct = _context.CustomerProducts
                .FirstOrDefault(cp => cp.CustomerId == customerId && cp.ProductId == productId);

            if (customerProduct != null)
            {
                _context.CustomerProducts.Remove(customerProduct);
                _context.SaveChanges();
            }
        }

    }
}
