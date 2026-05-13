using DPP.InternalWebhookHost.Application.Behaviors;
using DPP.InternalWebhookHost.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection; 
using System.Reflection; 

namespace DPP.InternalWebhookHost.Application.Extensions;

public static class ApplicationServiceExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
		services.AddValidatorsFromAssemblyContaining<GetWebhookReportValidator>();

		services.AddHttpClient();

		return services;
	}
}
