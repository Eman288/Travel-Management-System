using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPro.Models
{
    public class Bus
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]

        public int BusID { get; set; }

        [Required]
        [Display(Name = "Number of Seats")]
        public int NumberOfSeats { get; set; }
        public ICollection<Trip> Trip { get; set; }

    }
}
