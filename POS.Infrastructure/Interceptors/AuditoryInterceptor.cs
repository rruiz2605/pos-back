using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using POS.Domain.Entities;
using POS.Infrastructure.Constants;

namespace POS.Infrastructure.Interceptors
{
    public class AuditoryInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditoryFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateAuditoryFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateAuditoryFields(DbContext? context)
        {
            if (context == null)
            {
                return;
            }


            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.RecordStatus = DbConstants.RegisterStatus.ACTIVE;
                        entry.Entity.CreationDate = DateTime.Now;
                        entry.Entity.CreationUser = "userSession.Id";
                        entry.Entity.CreationTerminal = "userSession.Terminal";
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModificationDate = DateTime.Now;
                        entry.Entity.ModificationUser = "userSession.Id";
                        entry.Entity.ModificationTerminal = "userSession.Terminal";
                        break;
                }
            }
        }
    }
}
