namespace ItlaNetwork.Core.Domain.Settings
{
    public class MailSettings
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string DisplayName { get; set; }
        public string EmailFrom { get; set; }
    }
}