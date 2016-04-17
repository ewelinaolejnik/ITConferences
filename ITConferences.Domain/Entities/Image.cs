using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITConferences.Domain.Entities
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageID { get; set; }

        [Required]
        [Display(Name = "Image:")]
        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}