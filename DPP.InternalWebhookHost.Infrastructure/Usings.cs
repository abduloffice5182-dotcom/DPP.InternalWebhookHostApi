global using System.Data;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Data.SqlClient; 
global using Microsoft.Extensions.Configuration;

global using Dapper;

global using DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;
global using DPP.InternalWebhookHost.Domain.Entities.Response.Webhook;
global using DPP.InternalWebhookHost.Infrastructure.Interfaces;
global using DPP.InternalWebhookHost.Infrastructure.Persistence;
global using DPP.InternalWebhookHost.Infrastructure.Repositories;
global using DPP.InternalWebhookHost.Infrastructure.Constants.Configuration;
global using DPP.InternalWebhookHost.Infrastructure.Constants.DatabaseQueries.Webhook;
