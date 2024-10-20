using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Contracts.Persistence;
using POS.Infrastructure.Contexts;
using POS.Infrastructure.Interceptors;
using POS.Infrastructure.Repositories;

namespace POS.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<GeneralContext>(options =>
                options.UseMySQL(connectionString!)
            );
            services.AddScoped<AuditoryInterceptor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return services;
        }
    }
}
