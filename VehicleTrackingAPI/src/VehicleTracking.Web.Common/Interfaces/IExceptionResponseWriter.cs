using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace VehicleTracking.Web.Common.Interfaces {
    public interface IExceptionResponseWriter
    {
        Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code, string clientMessage);
    }
}