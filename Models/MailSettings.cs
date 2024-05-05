namespace PoepleDirectoryApplication.Api.Models
{
    public class MailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
    }
}
