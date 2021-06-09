using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using VehicleTracking.Interfaces.Correlation;

namespace VehicleTracking.Web.Common.Middleware {
    public class CorrelationIdMiddleware
    {
        string ExposeHeaders = "Access-Control-Expose-Headers";
        string CorrelationId = "CorrelationId";
        private readonly RequestDelegate _next;
        private readonly ICorrelationService _correlationService;
        public CorrelationIdMiddleware(RequestDelegate next, ICorrelationService correlationService)
        {
            _next = next;
            _correlationService = correlationService;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            var correlationId = Guid.NewGuid().ToString();

            if (request.Headers.ContainsKey(CorrelationId))
            {
                correlationId = request.Headers[CorrelationId];
            }

            _correlationService.Set(correlationId);

            if(!request.Headers.ContainsKey(CorrelationId))
                request.Headers.Add(CorrelationId, correlationId);

            context.Response.Headers.Add(CorrelationId, correlationId);
            context.Response.Headers.Add(ExposeHeaders, CorrelationId);

            await _next.Invoke(context);
        }
    }
}
