using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCPro.Models
{
    public class TouristAttraction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]

        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [Column(TypeName = "varchar(250)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "City")]
        [Column(TypeName = "varchar(100)")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [Column(TypeName = "varchar(1000)")] 
        public string Description { get; set; }=string.Empty;

        [Required]
        [Display(Name = "Picture")]
        [Column(TypeName = "varchar(250)")] 
        public string Picture { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile TouristAttractionPicture { get; set; }
        public ICollection<TripAtt> TripAtt { get; set; }
    }
}
