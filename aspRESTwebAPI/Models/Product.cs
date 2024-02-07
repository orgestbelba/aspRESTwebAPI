namespace aspRESTwebAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ICollection<CustomerProduct> CustomerProducts { get; set; }
    }
}
