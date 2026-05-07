using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using DPP.InternalWebhookHost.Infrastructure.Persistence;
using DPP.InternalWebhookHost.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DPP.InternalWebhookHost.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for registering infrastructure services.
    /// </summary>
    public static class InfrastructureServiceExtensions
    {
        /// <summary>
        /// Registers infrastructure-related services, such as repositories and database connection factories, into the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register all Repository
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient<IWebhookRepository, WebhookRepository>(); 

            return services;
        }
    }
}
