using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITConferences.Domain.Entities
{
    public class Speaker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpeakerID { get; set; }

        [DisplayName("Spoken conferences:")]
        public virtual ICollection<Conference> SpokenConferences { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }

        [Required]
        public virtual Attendee User { get; set; }
    }
}