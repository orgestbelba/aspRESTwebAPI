using aspRESTwebAPI.Models;

namespace aspRESTwebAPI.Interfaces
{
    public interface ICustomerRepository
    {
        ICollection<Customer> GetCustomers();
        Customer GetCustomer(int customerId);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerId);
        bool CustomerExists(int customerId);

        //Methods regarding the One-To-Many relationship
        ICollection<Order> GetOrdersForCustomer(int customerId);

        //Methods regarding the Many-To-Many relationship
        ICollection<Product> GetProductsForCustomer(int customerId);
        void AddProductToCustomer(int customerId, int productId);
        void RemoveProductFromCustomer(int customerId, int productId);
    }
}
