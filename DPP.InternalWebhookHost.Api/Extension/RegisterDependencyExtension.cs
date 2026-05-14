namespace DPP.InternalWebhookHost.Api.Extension;
public static class RegisterDependency
{
	public static void RegisterDI(this IServiceCollection services, Serilog.ILogger logger)
	{
		services.AddSingleton(logger);
		services.AddInfrastructureServices();
		services.AddApplicationServices();
	}
}

