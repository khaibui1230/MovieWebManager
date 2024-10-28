using Microsoft.EntityFrameworkCore;
using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this._db = dbContext;
            this.DbSet = dbContext.Set<T>();
            //db.Catgorry == DSSet
            _db.Products.Include(u => u.Category).Include(u => u.cateGoryId);
        }
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includesProperties = null, bool tracked = false)
        {
            IQueryable<T> values;
            //add the track to folow the Cart
            if (tracked)
            {
                values = DbSet;
            }
            else
            {
                values = DbSet.AsNoTracking();
            }

            values = values.Where(filter);
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
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includesProperties = null)
        {

            IQueryable<T> values = DbSet;
            if (filter != null)
            {
                values = values.Where(filter);
            }

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
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }
    }
}
