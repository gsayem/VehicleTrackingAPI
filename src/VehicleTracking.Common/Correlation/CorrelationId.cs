using VehicleTracking.Interfaces.Correlation;

namespace VehicleTracking.Common.Correlation {
    public class CorrelationId : ICorrelationId {
        public CorrelationId(string value) {
            Value = value;
        }

        public string Value { get; }
    }
}
