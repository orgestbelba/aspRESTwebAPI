using System.ComponentModel.DataAnnotations;

namespace aspRESTwebAPI.Dto
{
    public class WriteProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
