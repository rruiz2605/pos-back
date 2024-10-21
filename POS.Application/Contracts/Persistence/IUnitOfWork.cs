using POS.Domain.Entities;

namespace POS.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        IBaseRepository<Client> ClientRepository { get; }
        Task<int> Complete();
    }
}
