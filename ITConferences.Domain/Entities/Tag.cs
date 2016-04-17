using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITConferences.Domain.Entities
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagID { get; set; }

        [Display(Name = "Topics:")]
        public string Name { get; set; }

        public virtual ICollection<Conference> Conferences { get; set; }
    }
}