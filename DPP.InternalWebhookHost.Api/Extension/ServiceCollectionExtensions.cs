namespace DPP.InternalWebhookHost.Api;
public static class ServiceCollectionExtensions
{
	public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		services.AddEndpointsApiExplorer();
		services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition =
			System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
	});

		AddSwaggerGen(services);
		AddApiVersioning(services);
		AddCors(services, configuration);
		ConfigureApplicationCookie(services);
		ConfigureHealthChecks(services);
		AddCompression(services);
	}

	private static void ConfigureHealthChecks(IServiceCollection services)
	{
		services.AddHealthChecks();
	}

	private static void ConfigureApplicationCookie(IServiceCollection services)
	{
		services.ConfigureApplicationCookie(options =>
		{
			options.Cookie.SameSite = SameSiteMode.Lax;
		});
	}

	private static void AddApiVersioning(this IServiceCollection services)
	{
		services.AddApiVersioning(options =>
		{
			options.DefaultApiVersion = new ApiVersion(1);
			options.ReportApiVersions = true;
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.ApiVersionReader = ApiVersionReader.Combine(
			new UrlSegmentApiVersionReader(),
			new HeaderApiVersionReader("Api-Version"));
		})
		 .AddApiExplorer(options =>
		 {
			 options.GroupNameFormat = "'v'V";
			 options.SubstituteApiVersionInUrl = true;
		 });
	}
	private static void AddSwaggerGen(IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			var termsOfService = "https://www.deluxe.com/policy/";
			var contactURL = "https://www.deluxe.com/about/contact-us/";
			// Inject API version descriptions
			var provider = services.BuildServiceProvider()
								   .GetRequiredService<IApiVersionDescriptionProvider>();

			foreach (var description in provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, new OpenApiInfo
				{
					Title = $"DPP Webhook Integration Microservice V{description.ApiVersion}",
					Version = description.GroupName,
					Description = "Webhook Integration",
					TermsOfService = new Uri(termsOfService),
					Contact = new OpenApiContact
					{
						Name = "Deluxe®",
						Email = "contact@deluxe.com",
						Url = new Uri(contactURL)
					}
				});
			}
		});

	}

	private static void AddCors(IServiceCollection services, IConfiguration configuration)
	{
		services.AddCors(c =>
		{
			c.AddPolicy("AllowCORS", options =>
			{
				var origins = configuration.GetSection("CORS:AllowedOrigins").Get<string[]>();
				if (origins != null && origins.Length > 0)
				{
					options.WithOrigins(origins)
						   .AllowAnyHeader()
						   .AllowAnyMethod()
						   .AllowCredentials();
				}
				else
				{
					options.AllowAnyHeader()
						   .AllowAnyMethod()
						   .AllowCredentials();
				}
			});
		});
	}
	static void AddCompression(IServiceCollection services)
	{
		services.AddResponseCompression(options =>
		{
			options.EnableForHttps = true;
			options.Providers.Add<GzipCompressionProvider>();
		});

		services.Configure<GzipCompressionProviderOptions>(options =>
		{
			options.Level = CompressionLevel.Fastest;
		});
	}
}
