using Microsoft.EntityFrameworkCore;
using POS.Application.Contracts.Persistence;
using POS.Application.Models.Persistence;
using POS.Domain.Entities;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace POS.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly GeneralContext _context;

        public BaseRepository(GeneralContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(uint id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> CreateQuery(Expression<Func<T, bool>> predicate = null,
                                       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                       bool disableTracking = true,
                                       params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

            query = query.Where(x => x.RecordStatus == DbConstants.RegisterStatus.ACTIVE);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query);


            return query;
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate = null,
                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                     bool disableTracking = true,
                                     params Expression<Func<T, object>>[] includes)
        {
            return await CreateQuery(predicate, orderBy, disableTracking, includes)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate = null,
                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                     bool disableTracking = true,
                                     params Expression<Func<T, object>>[] includes)
        {
            return await CreateQuery(predicate, orderBy, disableTracking, includes)
                .ToListAsync();
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<PaginationResponse<T>> ListPaginatedAsync(int page, int pageSize,
                                     Expression<Func<T, bool>> predicate = null,
                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                     bool disableTracking = true,
                                     params Expression<Func<T, object>>[] includes)
        {
            var query = CreateQuery(predicate, orderBy, disableTracking, includes);

            var pagination = new PaginationResponse<T>() { Total = await query.CountAsync() };

            if (page > 0 && pageSize > 0)
            {
                var skip = (page - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);
            }
            
            pagination.Content = await query.ToListAsync();

            return pagination;
        }
    }
}
