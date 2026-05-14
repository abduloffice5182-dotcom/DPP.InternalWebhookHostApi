

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition =
			System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
	});
builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
});

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(configuration)
	.CreateLogger();

builder.Host.UseSerilog(Log.Logger);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDI(Log.Logger);
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
if (app.Environment.IsDevelopment())
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
