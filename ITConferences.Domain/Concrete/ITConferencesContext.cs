using ITConferences.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Concrete
{
    public class ITConferencesContext :DbContext
    {
        public ITConferencesContext() : base("ITConferences")
        {

        }

        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Inspiration> Media { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
