using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCPro.Models
{
    public class RegisterModel

    {
        [Key]
        [Display(Name = "National ID")]
        [Column(TypeName = "varchar(14)")]
        [MaxLength(14)]
        [MinLength(14)]
        public string NationalId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email")]
        [Column(TypeName = "varchar(250)")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Name")]
        [Column(TypeName = "varchar(250)")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Picture")]
        [Column(TypeName = "varchar(250)")]
        public string ImageUser { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Job")]
        [Column(TypeName = "varchar(250)")]
        public string Job { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nationality")]
        [Column(TypeName = "varchar(250)")]
        public string Nationality { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Password")]
        [Column(TypeName = "varchar(250)")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
  
}
