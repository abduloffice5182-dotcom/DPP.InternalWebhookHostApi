namespace DPP.InternalWebhookHost.Api.Extension;
public static class RegisterDependency
{
	public static void RegisterDI(this IServiceCollection services)
	{ 
		services.AddInfrastructureServices();
		services.AddApplicationServices();
	}
}

