using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int? id, string idStr = null);
        void InsertAndSubmit(T entity);
        void UpdateAndSubmit();
        void DeleteAndSubmit(T entity);
        void DisposeDataContext();
    }
}
