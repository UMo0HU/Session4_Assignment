using System.ComponentModel.DataAnnotations;

namespace Assignment_2.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Length(3, 30)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
