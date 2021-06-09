using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.IdentityServer.Web.Middleware {
    public class Prevent302Middleware
    {
        private readonly RequestDelegate _next;

        public Prevent302Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            await _next(ctx);
            if (ctx.Request.IsHttps && ctx.Response.StatusCode == 302 && ctx.Response.Headers.Any(x => x.Key == "Location"))
            {
                ctx.Response.Headers["Location"] = "https://" + (ctx.Request.Host + ctx.Response.Headers["Location"].ToString());
            }
        }
    }
}
