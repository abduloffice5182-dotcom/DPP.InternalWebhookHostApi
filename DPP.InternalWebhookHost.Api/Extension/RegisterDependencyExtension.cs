using DPP.InternalWebhookHost.Application.Extensions;
using DPP.InternalWebhookHost.Infrastructure.Extensions;

namespace DPP.InternalWebhookHost.Api.Extension;
	public static class RegisterDependency
	{
		/// <summary>
		/// Registers the application's dependencies and infrastructure services.
		/// </summary>
		/// <param name="services">The service collection to register services with.</param>
		/// <param name="logger">The Serilog logger instance to register as a singleton.</param>
		public static void RegisterDI(this IServiceCollection services, Serilog.ILogger logger)
		{
			services.AddSingleton(logger);
			services.AddInfrastructureServices();
		services.AddApplicationServices();
		}
	}

