var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

SqlMapper.AddTypeHandler(new JsonElementTypeHandler());
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(configuration)
	.CreateLogger();

builder.Host.UseSerilog(Log.Logger);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDI();
builder.Services.RegisterServices(configuration);
builder.Services.AddRouting(options =>
{
	options.LowercaseUrls = true;
});


//default value 30Mb
builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestBodySize = (configuration.GetValue<int?>(ApiConfigurationConstant.MaximumRequestSizeMB) ?? 30) * 1024 * 1024; 
});
 
var app = builder.Build();
app.UseSerilogRequestLogging();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
if (configuration.GetValue<bool?>(ApiConfigurationConstant.SwaggerEnable) ?? false)
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCompression();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
