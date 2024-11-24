using System.ComponentModel.DataAnnotations;

namespace EDCF_MVC.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
