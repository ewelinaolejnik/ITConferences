using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Attendee
    {
        public int AttendeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Attendee> Friends { get; set; }
        public virtual ICollection<Conference> FavouriteConferences { get; set; }

    }
}
