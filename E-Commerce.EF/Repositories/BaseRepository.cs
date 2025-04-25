using System.Linq.Expressions;
using E_Commerce.Core.Interfaces;
using E_Commerce.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDBContext _context;
        public BaseRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRanged(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().AnyAsync(predicate);
        

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().CountAsync(predicate);
        

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().Where(predicate).ToListAsync();
        

        public async Task<IEnumerable<T>> FindIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _context.Set<T>().Where(predicate);
            foreach (var property in includeProperties) { 
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }

        public async Task<T> FindSingle(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<T>> GetAll()
            => await _context.Set<T>().ToListAsync();
        


        public async Task<T> GetById(int id)
            => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetOrderedAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool ascending = true)
        {
            var query = _context.Set<T>().Where(predicate);

            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number must be ≥ 1", nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentException("Page size must be ≥ 1", nameof(pageSize));

            return await _context.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate)
        {

            if (pageNumber < 1) throw new ArgumentException("Page number must be ≥ 1", nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentException("Page size must be ≥ 1", nameof(pageSize));

            var query = _context.Set<T>().Where(predicate);
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }


        public void RemoveRanged(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }
    }
}
