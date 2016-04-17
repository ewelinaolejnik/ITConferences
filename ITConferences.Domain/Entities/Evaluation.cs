using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITConferences.Domain.Entities
{
    public class Evaluation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvaluationID { get; set; }

        [Range(1, 5)]
        public int CountOfStars { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Comment cannot be longer than 100 characters.")]
        public string Comment { get; set; }

        public virtual Attendee Owner { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
    }
}