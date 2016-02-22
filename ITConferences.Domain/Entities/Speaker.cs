using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Speaker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpeakerID { get; set; }

        public virtual ICollection<Conference> SpokenConferences { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }

        [Required]
        public virtual Attendee User { get; set; }

        //[ForeignKey("User")]
        //public string UserId { get; set; }
    }
}
