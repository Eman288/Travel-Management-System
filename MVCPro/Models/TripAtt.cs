using static MVCPro.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCPro.Models
{
    public class TripAtt
    {
        [Key]
        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public Trip? Trip { get; set; }

        [Key]
        [ForeignKey("TouristAttraction")]
        public int TouristAttractionId { get; set; }
        public TouristAttraction? TouristAttraction { get; set; }
        
        [Key]
        public int Order { get; set; }
    }
}
