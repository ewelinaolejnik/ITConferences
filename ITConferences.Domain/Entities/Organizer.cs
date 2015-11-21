using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Organizer : Attendee
    {
        public virtual ICollection<Conference> OrganizedConferences { get; set; }
    }
}
