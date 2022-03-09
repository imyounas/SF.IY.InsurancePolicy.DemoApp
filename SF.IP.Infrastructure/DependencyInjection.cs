using SF.IP.Application.Interfaces.Cache;
using SF.IP.Application.Interfaces.Database;
using SF.IP.Application.Interfaces.MessageQueue;
using SF.IP.Infrastructure.Cache;
using SF.IP.Infrastructure.Database;
using SF.IP.Infrastructure.MessageQueueHandler;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SF.IP.Application.Common;
using SF.IP.Infrastructure.StateRegulation;
using SF.IP.Application.Interfaces.StateRegulation;

namespace SF.IP.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services , AppSettings settings)
        {
           
            if (settings.UseInMemoryDatabase)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(settings.InMemoryDatabaseName));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        settings.ConnectionStrings.DefaultConnection,
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMQPooledObjectPolicy>();
            services.AddSingleton<IMQPublisher, MQPublisher>();
            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddScoped<IPolicyStateRegulator, PolicyStateRegulator>();

            return services;
        }
    }
}
