using Microsoft.Extensions.DependencyInjection;
using System;
using VehicleTracking.Common.Correlation;
using VehicleTracking.Interfaces;
using VehicleTracking.Interfaces.Correlation;

namespace VehicleTracking.Web.Common {
    public static class ServiceFactory {
        private static IServiceProvider _serviceProvider;
        private static IApplicationService _applicationService;

        public static void AddServiceProvider(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public static void AddApplicationService(IApplicationService applicationService) {
            _applicationService = applicationService;
        }

        public static IApplicationService GetApplicationService() {
            return _applicationService;
        }

        public static ICorrelationService GetCorrelationService() {
            return _serviceProvider == null ? new CorrelationService(null) : _serviceProvider.GetService<ICorrelationService>();
        }
    }
}
