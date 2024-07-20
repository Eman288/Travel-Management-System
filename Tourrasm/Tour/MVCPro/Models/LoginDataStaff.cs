using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCPro.Models
{
    public class LoginDataStaff
    {
        [Required]
        [Display(Name = "Email")]
        [Column(TypeName = "varchar(250)")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [Column(TypeName = "varchar(250)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "StaffCode")]
        [Column(TypeName = "varchar(250)")]
        public string StaffCode { get; set; }
    }
}
