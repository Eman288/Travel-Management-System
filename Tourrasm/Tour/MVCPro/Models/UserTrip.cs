using static MVCPro.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCPro.Models
{
    public class UserTrip
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Key]
        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public Trip Trip { get; set; }

        [Display(Name = "Book Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime BookDate { get; set; }
    }
}
