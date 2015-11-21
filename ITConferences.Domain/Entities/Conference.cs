using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Conference
    {
        public int ConferenceID { get; set; }
        public string Name { get; set; }
        public int TargetCityId { get; set; }
        public int TargetCountryId { get; set; }
        public virtual City TargetCity { get; set; }
        public virtual Country TargetCountry { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public bool IsPaid { get; set; }

        public virtual ICollection<Attendee> Attendees { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
        public virtual ICollection<Organizer> Organizers { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Inspiration> Media { get; set; }
    }
}
