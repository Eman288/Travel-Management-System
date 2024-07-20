using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPro.Models
{
    public class LoginData
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

    }
}