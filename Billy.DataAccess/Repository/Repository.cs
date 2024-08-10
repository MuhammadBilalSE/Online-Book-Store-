using Billy.DataAccess.Data;
using Billy.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Billy.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BulkyDB db;
        internal readonly DbSet<T> dBSet;
        public Repository(BulkyDB _db)
        {
            db = _db;
            this.dBSet = db.Set<T>();
            db.Products.Include(x => x.category);
        }
        public void Add(T entity)
        {
            dBSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked= false)
        {
            IQueryable<T> query;
            if (tracked)
            {
               query = dBSet;
            }
            else
            {
                query = dBSet.AsNoTracking();
            }
                query = query.Where(filter);
                
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var item in includeProperties
                        .Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(item);
                    }
                }

                return query.FirstOrDefault();
            

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties=null)
        {
            IQueryable<T> query = dBSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var item in includeProperties
                    .Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                   query = query.Include(item);
                }
            } 
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dBSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dBSet.RemoveRange(entities);
        }
    }
}
