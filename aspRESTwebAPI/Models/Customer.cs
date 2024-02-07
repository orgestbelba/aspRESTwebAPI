using System.Text.Json.Serialization;

namespace aspRESTwebAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<CustomerProduct> CustomerProducts { get; set; }
    }
}
