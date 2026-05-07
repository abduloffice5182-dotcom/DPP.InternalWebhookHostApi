using DPP.InternalWebhookHost.Domain.Common.Response;
using System.Net;

namespace DPP.InternalWebhookHost.Domain.Common.ExceptionHandling;
    public static class ApiErrorHandler
    {
        public static ApiError GetDefaultErrorMessages(HttpStatusCode statusCode, string customMessages)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => new ApiError { Message = "Bad Request", Details = string.IsNullOrWhiteSpace(customMessages) ? HttpErrorMessages.BadRequest : customMessages },
                HttpStatusCode.Unauthorized => new ApiError { Message = "Unauthorized", Details = string.IsNullOrWhiteSpace(customMessages) ? HttpErrorMessages.Unauthorized : customMessages },
                HttpStatusCode.Forbidden => new ApiError { Message = "Forbidden", Details = string.IsNullOrWhiteSpace(customMessages) ? HttpErrorMessages.Forbidden : customMessages },
                HttpStatusCode.InternalServerError => new ApiError { Message = "Internal Server Error", Details = string.IsNullOrWhiteSpace(customMessages) ? HttpErrorMessages.InternalServerError : customMessages },
                _ => new ApiError { Message = "Bad Request", Details = string.IsNullOrWhiteSpace(customMessages) ? HttpErrorMessages.UnexpectedError : customMessages }
            };
        }

    }
