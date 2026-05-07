using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Domain.Common.ExceptionHandling;

    public static class HttpErrorMessages
    {
        public const string BadRequest = "An error occurred while processing your request.";
        public const string Unauthorized = "You are not authorized to access this resource.";
        public const string Forbidden = "You are not authorized to access this resource.";
        public const string InternalServerError = "An internal server error occurred.";
        public const string UnexpectedError = "An unexpected error occurred.";
    }
