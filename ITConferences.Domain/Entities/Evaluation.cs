using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Evaluation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvaluationID { get; set; }

        [Range(0, 5)]
        public int CountOfStars { get; set; }

        [StringLength(100,MinimumLength = 3, ErrorMessage = "Comment cannot be longer than 100 characters.")]
        public string Comment { get; set; }

       
       // public virtual Conference Conference { get; set; }
       // //TODO: speaker evaluation
       //// public virtual Speaker Speaker { get; set; }

       // [ForeignKey("Conference")]
       // public int? ConferenceRefId { get; set; }

       // //[ForeignKey("Speaker")]
       // //public int SpeakerRefId { get; set; }
    }
}
