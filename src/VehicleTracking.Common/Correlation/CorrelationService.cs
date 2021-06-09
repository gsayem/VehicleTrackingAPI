using Microsoft.AspNetCore.Http;
using System.Threading;
using VehicleTracking.Interfaces.Correlation;

namespace VehicleTracking.Common.Correlation {
    public class CorrelationService : ICorrelationService {
        private readonly IHttpContextAccessor _contextAccessor;
        private const string Key = "CorrelationId";
        private readonly object _syncLock = new object();
        private static readonly AsyncLocal<string> AsyncCorrelationId = new AsyncLocal<string>();

        public CorrelationService() {
        }

        public CorrelationService(IHttpContextAccessor contextAccessor) {
            _contextAccessor = contextAccessor;
        }

        public void Set(string correlationId) {
            if (_contextAccessor?.HttpContext != null) {
                lock (_syncLock) {
                    _contextAccessor.HttpContext.Items[Key] = correlationId;
                }
            } else {
                AsyncCorrelationId.Value = correlationId;
            }
        }

        public string Get() {
            if (_contextAccessor?.HttpContext != null) {
                if (_contextAccessor.HttpContext.Items.ContainsKey(Key))
                    return _contextAccessor.HttpContext.Items[Key].ToString();

                return string.Empty;
            }
            return AsyncCorrelationId.Value;
        }
    }
}
