namespace VehicleTracking.Interfaces.Correlation {
    public interface ICorrelationService {
        void Set(string correlationId);
        string Get();
    }
}
