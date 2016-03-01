using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITConferences.Domain.Entities;
using System.Data.Entity.Infrastructure;

namespace ITConferences.Domain.Abstract
{
    public interface IDataContext
    {
        IDbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T item) where T : class;
        int SaveChanges();
        void Dispose();
        IDbSet<Conference> Conferences { get; set; }
        IDbSet<Country> Countries { get; set; }
    }
}
