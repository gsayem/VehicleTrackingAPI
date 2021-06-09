namespace VehicleTracking.Web.Common {
    public class EmailSettings {
        public bool Enable { get; set; }
        public string EmailSenderName { get; set; }
        public string EmailSender { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassowrd { get; set; }
        public SMTP SMTP { get; set; }
    }
    public class SMTP {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool Secure { get; set; }
    }
}
