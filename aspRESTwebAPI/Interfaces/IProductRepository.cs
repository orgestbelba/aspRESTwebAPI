using aspRESTwebAPI.Models;

namespace aspRESTwebAPI.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        Product GetProduct(int productId);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productId);
        bool ProductExists(int productId);

        // Methods regarding the Many-to-Many relationship
        ICollection<Customer> GetCustomersForProduct(int productId);
        void AddCustomerToProduct(int productId, int customerId);
        void RemoveCustomerFromProduct(int productId, int customerId);
    }
}
