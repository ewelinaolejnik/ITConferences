using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Concrete
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext() : base("ITConferencesDb")
        {

        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            return this.SaveChanges();
        }

        public IDbSet<Conference> Conferences { get; set; }
        public IDbSet<Attendee> Attendees { get; set; }
        public IDbSet<Organizer> Organizers { get; set; }
        public IDbSet<Speaker> Speakers { get; set; }
        public IDbSet<Inspiration> Media { get; set; }
        public IDbSet<Evaluation> Evaluations { get; set; }
        public IDbSet<Country> Countries { get; set; }
        public IDbSet<City> Cities { get; set; }
        public IDbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public void ExecuteCommand(string command, params object[] parameters)
        {
            base.Database.ExecuteSqlCommand(command, parameters);
        }
    }
}
