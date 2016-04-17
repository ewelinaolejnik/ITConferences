using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITConferences.Domain.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityID { get; set; }

        [Required]
        [Display(Name = "City:")]
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public virtual Country Country { get; set; }
    }
}