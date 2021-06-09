using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using VehicleTracking.Web.Common.Interfaces;

namespace VehicleTracking.IdentityServer.Web.API.Infrastructure {
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionResponseWriter _exceptionResponseWriter;

        public ExceptionHandler(IExceptionResponseWriter exceptionResponseWriter)
        {
            _exceptionResponseWriter = exceptionResponseWriter;
        }
        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;
            var code = HttpStatusCode.InternalServerError;

            // Check for specific exeption types here and
            // set "code" to appropriate HttpStatusCode
            //var clientMessage = $"Message:{exception.Message}\n InnerException:{exception.InnerException.Message}\nStackTrace:{exception.StackTrace}";// "Internal server error";
            var clientMessage = "Internal server error";

            await _exceptionResponseWriter.WriteExceptionAsync(context, exception, code, clientMessage);
        }
    }
}
