using POS.Application.Models.Persistence;
using POS.Domain.Entities;
using System.Linq.Expressions;

namespace POS.Application.Contracts.Persistence
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T?> GetByIdAsync(long id);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate = null,
                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                     bool disableTracking = true,
                                     params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate = null,
                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                     bool disableTracking = true,
                                     params Expression<Func<T, object>>[] includes);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        PaginationResponse<T> ListPaginatedAsync(IReadOnlyList<T> query, int page, int pageSize);
    }
}
