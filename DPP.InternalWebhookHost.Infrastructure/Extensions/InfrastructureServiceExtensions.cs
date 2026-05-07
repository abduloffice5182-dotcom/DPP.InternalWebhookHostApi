using DPP.PartnerPaymentIntegration.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DPP.PartnerPaymentIntegration.Infrastructure.Extensions
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
            services.AddTransient<IMerchantRepository, MerchantRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddSingleton<FortellisTokenResponse>();
            services.AddScoped<IFortellisAPIClient, FortellisAPIClient>();
            services.AddTransient<IFortellisIntegration, FortellisIntegration>();
            services.AddTransient<IFortellisRepositories, FortellisRepositories>();

            return services;
        }
    }
}
