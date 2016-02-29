using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Organizer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganizerID { get; set; }

        [DisplayName("Organized conferences:")]
        public virtual ICollection<Conference> OrganizedConferences { get; set; }
      
        [Required]
        public virtual Attendee User { get; set; }
    }
}
