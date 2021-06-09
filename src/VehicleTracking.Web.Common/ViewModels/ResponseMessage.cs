namespace VehicleTracking.Web.Common.ViewModels {
    public enum Status {
        SUCCESS,
        FAILURE,
        WARNING
    }
    public enum Code {
        GET,
        CREATE,
        UPDATE,
        DELETE,
        DATA_ALREADY_EXIST,
        DATA_NOT_PROVIDED,
        DATA_NOT_FOUND,
    }
    public class ResponseMessage<T> {
        public ResponseMessage() {

        }
        public ResponseMessage(Status status, Code code) {
            Status = status.ToString();
            Code = code.ToString();
        }
        public ResponseMessage(Status status, Code code, string message) : this(status, code) {
            Message = message;
        }
        public ResponseMessage(Status status, Code code, string message, T data) : this(status, code, message) {
            Data = data;
        }
        public string Code { get; set; }
        public string Status { get; set; }

        public string Message { get; set; }
        public T Data { get; set; }
    }
}
