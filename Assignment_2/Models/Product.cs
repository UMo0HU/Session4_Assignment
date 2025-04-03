using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_2.Models
{
    public class Product
    {
        public int Id { get; set; }
        [DisplayName("Product Name")]
        [Length(3, 30)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Product Price.")]
        [Range(1, 50000)]
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string? Img { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public IFormFile ClientFile { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "Choose Category.")]
        [Range(1, int.MaxValue, ErrorMessage="Choose Category.")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<OrderItem>? OrderItems { get; set; }

    }
}
