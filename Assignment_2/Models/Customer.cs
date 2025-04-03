using System.ComponentModel.DataAnnotations;

namespace Assignment_2.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Length(3, 30)]
        public string Name { get; set; }
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
        public string Email { get; set; }
        public string Phone { get; set; }

    }
}
