using Microsoft.Extensions.DependencyInjection; 
using System.Reflection; 

namespace DPP.InternalWebhookHost.Application.Extensions;

public static class ApplicationServiceExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		// Register all MediatR handlers from Application layer
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		services.AddHttpClient();

		return services;
	}
}
