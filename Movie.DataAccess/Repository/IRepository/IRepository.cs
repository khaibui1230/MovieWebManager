﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie.DataAccess.Repository.IRepository
{
    public  interface IRepository<T> where T : class
    {
        //T Category
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includesProperties = null);
        T Get (Expression <Func<T, bool>> filter, string? includesProperties = null,bool tracked = false);
        void Add(T entity);
        void Remove (T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
