using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Humanizer.On;
using static Humanizer.OnDate;

namespace MVCPro.Models
{

    // [Table("Users", Schema = "dbo")]
    public class User
    {
        public class ApplicationUser : IdentityUser
        {
            public string NationalNumber { get; set; }
        }
        [Key]
        [Display(Name = "National ID")]
        [Column(TypeName = "varchar(14)")]
        // [MaxLength(14)]
        // [MinLength(14)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "Invalid National number, It must be only 14 digit")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "National number must contain only digits")]
        public string NationalId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Name")]
        [Column(TypeName = "varchar(250)")]
        [RegularExpression(@"^[a-z' 'A-Z]+$", ErrorMessage = "Invalid name!")]
        public string UserName { get; set; } = string.Empty;


        [Display(Name = "Picture")]
        [Column(TypeName = "varchar(250)")]
        public string ImageUser { get; set; } = string.Empty;



        [Required]
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [RegularExpression(@"^[a-z' 'A-Z]+$", ErrorMessage = "Invalid job")]
        [Display(Name = "Job")]
        [Column(TypeName = "varchar(250)")]
        public string? Job { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nationality")]
        [Column(TypeName = "varchar(250)")]
        [RegularExpression(@"^[a-z' 'A-Z]+$", ErrorMessage = "Invalid Nationality!")]
        public string Nationality { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Email")]
        [Column(TypeName = "varchar(250)")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)$", ErrorMessage = "Invalid Email Address.")]        // [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ICollection<UserTrip>? UserTrips { get; set; }
    }

}