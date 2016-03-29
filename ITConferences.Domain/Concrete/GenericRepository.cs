using ITConferences.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Concrete
{
    public class GenericRepository : IGenericRepository
    {
        private IDataContext _dataContext;

        public GenericRepository(IDataContext dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException("Datacontext is null");

            _dataContext = dataContext;
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _dataContext.Set<T>();
        }

        public T GetById<T>(int? id, string idStr = null) where T : class
        {
            if (idStr != null)
            {
                return _dataContext.Set<T>().Find(idStr);
            }
            return _dataContext.Set<T>().Find(id);
        }

        public void InsertAndSubmit<T>(T entity) where T : class
        {
            this._dataContext.Set<T>().Add(entity);
            _dataContext.SaveChanges();
        }

        public void UpdateAndSubmit<T>(T entity) where T : class
        {
            _dataContext.SaveChanges();
        }

        public void DeleteAndSubmit<T>(T entity) where T : class
        {
            _dataContext.Set<T>().Remove(entity);
            _dataContext.SaveChanges();
        }

        public void DisposeDataContext()
        {
            _dataContext.Dispose();
        }
    }
}
