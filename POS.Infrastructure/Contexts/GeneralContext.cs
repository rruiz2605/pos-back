using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Interceptors;

namespace POS.Infrastructure.Contexts
{
    public class GeneralContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        private readonly AuditoryInterceptor auditoryInterceptor;

        public GeneralContext(DbContextOptions<GeneralContext> options, AuditoryInterceptor auditoryInterceptor) : base(options)
        {
            this.auditoryInterceptor = auditoryInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(auditoryInterceptor);
        }
    }
}
