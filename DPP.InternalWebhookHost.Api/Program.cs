using DPP.InternalWebhookHost.Api.Middlewares;
using DPP.InternalWebhookHost.Application.Operations.Queries.Validators;
using FluentValidation;
using DPP.InternalWebhookHost.Application.Operations.Queries.Requests;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration; 
builder.Services.AddControllers(); 
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
builder.Services.AddValidatorsFromAssemblyContaining<GetWebhookReportValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDI(Log.Logger);
builder.Services.RegisterServices(configuration);
builder.Services.AddRouting(options =>
{
	options.LowercaseUrls = true;
});


var app = builder.Build();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
