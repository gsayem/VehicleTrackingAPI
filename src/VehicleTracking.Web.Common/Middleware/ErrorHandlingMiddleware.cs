using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using VehicleTracking.Web.Common.Interfaces;

namespace VehicleTracking.Web.Common.Middleware {
    public class ErrorHandlingMiddleware
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next, IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
            _next = next;

            TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
            {                
                var aggException = eventArgs.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                {
                    //Log.Error(exception, "Inner exemption in aggregate task exception");
                }
            };
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "Unhandled exception caught in middleware");
                await _exceptionHandler.HandleExceptionAsync(context, ex);
            }
        }
    }
}
