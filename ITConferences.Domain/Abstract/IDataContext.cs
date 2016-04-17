using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ITConferences.Domain.Entities;

namespace ITConferences.Domain.Abstract
{
    public interface IDataContext
    {
        IDbSet<Conference> Conferences { get; set; }
        IDbSet<Country> Countries { get; set; }
        IDbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T item) where T : class;
        int SaveChanges();
        void Dispose();
    }
}