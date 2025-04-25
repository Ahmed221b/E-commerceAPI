using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> FindSingle(Expression<Func<T, bool>> predicate);
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> AddAsync(T entity);
        T Update(T entity);
        void Remove(T entity);
        Task<IEnumerable<T>> AddRanged(IEnumerable<T> entities);
        void RemoveRanged(IEnumerable<T> entities);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T,bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetOrderedAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool ascending = true);
    }
}
