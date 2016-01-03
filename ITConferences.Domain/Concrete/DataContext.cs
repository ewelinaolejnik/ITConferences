using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ITConferences.Domain.Concrete
{
    public class DataContext : IdentityDbContext, IDataContext
    {
        public DataContext() : base("ITConferences")
        {

        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
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

        public static DataContext Create()
        {
            return new DataContext();
        }
    }
}
