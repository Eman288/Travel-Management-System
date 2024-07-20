using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPro.Models
{
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int TripId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [Column(TypeName ="varchar(250)")]
        public string Name { get; set; }= string.Empty;

        [Display(Name = "From Location")]
        [Column(TypeName = "varchar(250)")]
        [Required]
        public string FromLocation { get; set; }=string.Empty;

        [Display(Name = "Start Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "City")]
        [Column(TypeName = "varchar(250)")]
        [Required]
        public string City { get; set; }=string.Empty ;

        [Display(Name = "Price")]
        [Column(TypeName = "varchar(250)")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Description")]
        [Column(TypeName = "varchar(1000)")]
        public string Description { get; set; } = string.Empty;
        //User
        public ICollection<UserTrip> UserTrips { get; set; }

        [Display(Name = "Picture")]
        [Column(TypeName = "varchar(250)")]
        public string ImageUser { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile ProfilePicture { get; set; }

        //Bus
        [ForeignKey("BusID")]
        public int BusID { get; set; } // Foreign Key
        public Bus Bus { get; set; } // Navigation Property
        //TouristAttraction
        public ICollection<TripAtt> TripAtt { get; set; }

        //stuff
        [ForeignKey("staffID")]
        public string staff { get; set; }
        public Staff Staff { get; set; }
    }
}
