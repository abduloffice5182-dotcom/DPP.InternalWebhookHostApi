global using System.Reflection;
global using System.Text;
global using System.Net;
global using System.Text.Json.Serialization;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.OpenApi.Models;

global using Asp.Versioning;
global using Asp.Versioning.ApiExplorer;
global using MediatR; 
global using Serilog;

global using DPP.InternalWebhookHost.Api;
global using DPP.InternalWebhookHost.Api.Extension;
global using DPP.InternalWebhookHost.Application.Operations.Commands.Requests;
global using DPP.InternalWebhookHost.Application.Operations.Queries.Requests;
global using DPP.InternalWebhookHost.Domain.Common.Response;
global using DPP.InternalWebhookHost.Api.Middlewares;