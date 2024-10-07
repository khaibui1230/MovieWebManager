using Microsoft.EntityFrameworkCore;
using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this._db = dbContext;
            this.dbSet = dbContext.Set<T>();
            //db.Catgorry == DSSet
            _db.Products.Include(u => u.Category).Include(u => u.cateGoryId);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter, string? includesProperties = null)
        {
            IQueryable<T> values = dbSet.Where(filter);
            //check the include properties
            if (!string.IsNullOrEmpty(includesProperties))
            {
                foreach (var property in includesProperties.
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    values = values.Include(property);
                }
            }
            return values.FirstOrDefault();
        }

        //Category, CoverType
        public IEnumerable<T> GetAll(string? includesProperties = null)
        {
            IQueryable<T> values = dbSet;
            if (!string.IsNullOrEmpty(includesProperties))
            {
                foreach (var property in includesProperties.
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    values = values.Include(property);
                }
            }
            return values.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
