using Microsoft.EntityFrameworkCore;
using FrameworkBaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkBaseData.Models;
using System.Linq.Expressions;

namespace FrameworkBaseRepo
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly FrameworkBaseContext context;
        private DbSet<T> entities;
        private string errorMessage = string.Empty;

        public Repository(FrameworkBaseContext context)
        {
            try
            {
                this.context = context;
                entities = context.Set<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<T> GetAll()
        {
            try
            {
                return entities.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] properties)
        {
            try
            {
                if (properties == null)
                    throw new ArgumentNullException(nameof(properties));

                var query = entities as IQueryable<T>; // entities = dbContext.Set<TEntity>()

                query = properties.Aggregate(query, (current, property) => current.Include(property));

                return query.ToList(); //readonly
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Get(int id)
        {
            try
            {
                return entities.SingleOrDefault(s => s.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Get(int id, params Expression<Func<T, object>>[] properties)
        {
            try
            {
                if (properties == null)
                    throw new ArgumentNullException(nameof(properties));

                IQueryable<T> query = context.Set<T>();

                query = properties.Aggregate(query, (current, property) => current.Include(property));

                return query.AsNoTracking().FirstOrDefault(e => e.Id == id); //readonly
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entity.Base_Addeddate = DateTime.Now;
                entity.Base_Modifieddate = DateTime.Now;
                entity.Base_Ipaddress = "";
                entity.Base_Username = "";
                entities.Add(entity);

                context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> InsertRange(IEnumerable<T> entitiesCollection)
        {
            try
            {
                if (entitiesCollection == null)
                {
                    throw new ArgumentNullException("entitiesCollection");
                }

                entitiesCollection.ToList().ForEach(e =>
                {
                    e.Base_Addeddate = DateTime.Now;
                    e.Base_Modifieddate = DateTime.Now;
                    e.Base_Ipaddress = "";
                    e.Base_Username = "";
                });

                entities.AddRange(entitiesCollection);

                context.SaveChanges();

                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entity.Base_Modifieddate = DateTime.Now;
                entity.Base_Ipaddress = "";
                entity.Base_Username = "";
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                entities.Remove(entity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteRange(IEnumerable<T> entitiesCollection)
        {
            try
            {
                if (entitiesCollection == null)
                {
                    throw new ArgumentNullException("entitiesCollection");
                }
                entities.RemoveRange(entitiesCollection);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remove(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entities.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void TruncateTable(string tableName)
        {
            try
            {
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                await context.Database.ExecuteSqlCommandAsync(string.Format("TRUNCATE TABLE {0}", tableName));
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}