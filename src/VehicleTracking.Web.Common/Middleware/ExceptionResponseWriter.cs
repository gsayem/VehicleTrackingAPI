using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using VehicleTracking.Web.Common.Interfaces;

namespace VehicleTracking.Web.Common.Middleware {
    public class ExceptionResponseWriter : IExceptionResponseWriter
    {
        public async Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code, string clientMessage)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = new
                {
                    message = clientMessage,
                    exception = exception.GetType().Name
                }
            })).ConfigureAwait(false);
        }
    }
}
