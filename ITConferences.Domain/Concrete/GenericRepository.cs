using ITConferences.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private IDataContext _dataContext;

        public GenericRepository(IDataContext dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException("Datacontext is null");

            _dataContext = dataContext;
        }

        public IEnumerable<T> GetAll()
        {
            return _dataContext.Set<T>();
        }

        public T GetById(int? id)
        {
            return _dataContext.Set<T>().Find(id);
        }

        public void InsertAndSubmit(T entity)
        {
            this._dataContext.Set<T>().Add(entity);
            _dataContext.SaveChanges();
        }

        public void UpdateAndSubmit(T entity)
        {
            _dataContext.SaveChanges();
        }

        public void DeleteAndSubmit(T entity)
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
