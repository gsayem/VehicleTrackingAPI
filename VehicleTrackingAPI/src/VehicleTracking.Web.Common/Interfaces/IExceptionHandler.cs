using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace VehicleTracking.Web.Common.Interfaces {
    public interface IExceptionHandler
    {
        Task HandleExceptionAsync(HttpContext context, Exception exception);
    }
}
