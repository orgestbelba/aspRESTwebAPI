using System.ComponentModel.DataAnnotations;

namespace aspRESTwebAPI.Dto
{
    public class WriteOrderDto
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public double TotalAmount { get; set; }
    }
}
