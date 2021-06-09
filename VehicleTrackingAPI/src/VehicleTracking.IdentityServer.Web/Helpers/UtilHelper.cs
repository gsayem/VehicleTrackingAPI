using Microsoft.AspNetCore.Http;
using RazorLight;
using System.Dynamic;
using System.Threading.Tasks;
using VehicleTracking.IdentityServer.Services;

namespace VehicleTracking.IdentityServer.Web.API.Helpers {
    public class UtilHelper : IUtilHelper {
        IClientService _clientService;
        public UtilHelper(IClientService clientService) {
            _clientService = clientService;
        }
        public string BaseUrl(HttpRequest request) {
            return request.Scheme + "://" + request.Host;
        }

        public async Task<string> ParseTemplateAsync(string templateFileName, object model, dynamic expandoObject) {
            string basePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates");
            if (expandoObject == null) {
                expandoObject = new ExpandoObject();
            }
            expandoObject.BasePath = basePath;
            var engine = new RazorLightEngineBuilder().UseFileSystemProject(basePath).UseMemoryCachingProvider().Build();

            string result = await engine.CompileRenderAsync(templateFileName, model, typeof(object), expandoObject);
            return result;

        }
    }
}
