using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPro.Models
{
    public class Staff
    {
        [Key]
        [Display(Name = "National ID")]
        [Column(TypeName = "varchar(14)")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "Invalid National number, It must be only 14 digit")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "National number must contain only digits")]
        public string NationalId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [Column(TypeName = "varchar(250)")]
        [RegularExpression(@"^[a-z' 'A-Z]+$", ErrorMessage = "Invalid name!")]
        public string Name { get; set; }

        [Display(Name = "Picture")]
        [Column(TypeName = "varchar(250)")]
        public string ImageUser { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile ProfilePicture { get; set; }

        [Required]
        [Display(Name = "Email")]
        [Column(TypeName = "varchar(250)")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)$", ErrorMessage = "Invalid Email Address.")]        // [EmailAddress]

        public string Email { get; set; }

        [Required]
        [Display(Name = "Type")]
        [Column(TypeName = "varchar(250)")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Staff Code")]
        [Column(TypeName = "varchar(250)")]
        public string StaffCode { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]

        public string Password { get; set; }

        //trips
        public ICollection<Trip>? Trip { get; set; }

        //stuff unary relation
        //public ICollection<Staff> staff { get; set; }
        //[ForeignKey("StaffID")]
        //public int StaffID { get; set; } // Foreign Key
        //public Staff? staff { get; set; } // Navigation Property

    }
}