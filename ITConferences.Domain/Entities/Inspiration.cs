using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Inspiration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InspirationID { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid URL!")]
        [Display(Name = "Media address")]
        public string Url { get; set; }
    }
}
