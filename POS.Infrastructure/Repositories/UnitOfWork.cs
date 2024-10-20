using POS.Application.Contracts.Persistence;
using POS.Domain.Entities;
using POS.Infrastructure.Contexts;
using System.Collections;

namespace POS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable repositories;
        public readonly GeneralContext context;

        public UnitOfWork(GeneralContext context)
        {
            this.context = context;
        }

        public async Task<int> Complete()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (repositories == null)
            {
                repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), context);
                repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepository<TEntity>)repositories[type];
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
