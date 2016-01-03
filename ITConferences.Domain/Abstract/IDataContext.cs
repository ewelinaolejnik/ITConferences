using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITConferences.Domain.Entities;

namespace ITConferences.Domain.Abstract
{
    public interface IDataContext
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        void Dispose();
        IDbSet<Conference> Conferences { get; set; }
        IDbSet<Country> Countries { get; set; }
    }
}
