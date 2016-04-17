using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ITConferences.Domain.Concrete
{
    public class DataContext : IdentityDbContext, IDataContext
    {
        public DataContext() : base("ITConferences")
        {
        }

        public IDbSet<Attendee> Attendees { get; set; }
        public IDbSet<Organizer> Organizers { get; set; }
        public IDbSet<Speaker> Speakers { get; set; }
        public IDbSet<Evaluation> Evaluations { get; set; }
        public IDbSet<City> Cities { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<Image> Images { get; set; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public new DbEntityEntry<T> Entry<T>(T item) where T : class
        {
            return base.Entry(item);
        }


        public IDbSet<Conference> Conferences { get; set; }
        public IDbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public void ExecuteCommand(string command, params object[] parameters)
        {
            Database.ExecuteSqlCommand(command, parameters);
        }

        public static DataContext Create()
        {
            return new DataContext();
        }
    }
}