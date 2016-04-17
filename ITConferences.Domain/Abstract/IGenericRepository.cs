using System.Collections.Generic;

namespace ITConferences.Domain.Abstract
{
    public interface IGenericRepository
    {
        IEnumerable<T> GetAll<T>() where T : class;
        T GetById<T>(int? id, string idStr = null) where T : class;
        void InsertAndSubmit<T>(T entity) where T : class;
        void UpdateAndSubmit<T>(T entity) where T : class;
        void DeleteAndSubmit<T>(T entity) where T : class;
        void DisposeDataContext();
    }
}