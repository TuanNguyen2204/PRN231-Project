using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private ClothesStoreContext _ClothesStoreContext { get; set; }
        public RepositoryBase(ClothesStoreContext ClothesStoreContext)
        {
            _ClothesStoreContext = ClothesStoreContext;
        }
        public void Create(T entity)
        {
           _ClothesStoreContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _ClothesStoreContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return _ClothesStoreContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _ClothesStoreContext.Set<T>().Where(expression).AsNoTracking();
        }

        public IQueryable<T> Pagination(Expression<Func<T, bool>> filter, int pageNumber, int pageSize)
        {
            int skipCount = (pageNumber - 1) * pageSize;
            return _ClothesStoreContext.Set<T>()
                .Where(filter)
                .Skip(skipCount)
                .Take(pageSize)
                .AsNoTracking();
        }

        public void Update(T entity)
        {
            _ClothesStoreContext.Set<T>().Update(entity);
        }
    }
}
