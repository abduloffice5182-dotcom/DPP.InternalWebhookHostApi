global using System.Text.Json;
global using System.Reflection;

global using Microsoft.Extensions.DependencyInjection;

global using MediatR;
global using FluentValidation; 
 
global using DPP.InternalWebhookHost.Application.Common.Interfaces;
global using DPP.InternalWebhookHost.Application.Operations.Queries.Requests;
global using DPP.InternalWebhookHost.Application.Operations.Queries.Response.Webhook;
global using DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;
global using DPP.InternalWebhookHost.Infrastructure.Interfaces;
global using DPP.InternalWebhookHost.Application.Operations.Commands.Requests;
global using DPP.InternalWebhookHost.Application.Behaviors;
global using DPP.InternalWebhookHost.Application.Validators; 

