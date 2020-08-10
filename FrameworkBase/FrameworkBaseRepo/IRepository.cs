using FrameworkBaseData;
using FrameworkBaseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FrameworkBaseRepo
{
    public interface IRepository<T> where T : BaseModel
    {
        IQueryable<T> GetAll();

        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] properties);

        T Get(int id);

        T Get(int id, params Expression<Func<T, object>>[] properties);

        T Insert(T entity);

        IEnumerable<T> InsertRange(IEnumerable<T> entitiesCollection);

        void Update(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entitiesCollection);

        void Remove(T entity);

        void SaveChanges();

        void TruncateTable(string tableName);
    }
}