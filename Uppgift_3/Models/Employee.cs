using System.ComponentModel.DataAnnotations;

namespace Uppgift_3.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max name length is 100 characters")]
        public string EmployeeName { get; set; }
        //public string Email { get; set; }

        [Required(ErrorMessage = "You forgot to enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsAdmin { get; set; } 
    }
}
