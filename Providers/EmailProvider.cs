using System.Net;
using System.Net.Mail;
using PoepleDirectoryApplication.Api.Models;

namespace PoepleDirectoryApplication.Api.Providers
{
    public interface IEmailProvider
    {
        bool SendMail(MailData mailData);
    }

    public class EmailProvider : IEmailProvider
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailProvider> _logger;
        private readonly MailSettings _mailSettings;
        private readonly NetworkCredential _credentials;

        public EmailProvider(IConfiguration config, ILogger<EmailProvider> logger)
        {
            _config = config;
            _logger = logger;
            _mailSettings = GetMailSettings();
            _credentials = GetNetworkCredentials();
        }

        private MailSettings GetMailSettings()
            => new MailSettings
            {

                SmtpServer = _config.GetSection("MailSettings")["SmtpServer"],
                SmtpPort = int.Parse(_config.GetSection("MailSettings")["SmtpPort"]),
                ToAddress = _config.GetSection("MailSettings")["ToAddress"],
                FromAddress = _config.GetSection("MailSettings")["FromAddress"],
            };

        private NetworkCredential GetNetworkCredentials()
            => new NetworkCredential(
                _config.GetSection("MailSettings")["UserName"],
                _config.GetSection("MailSettings")["Password"]);

        public bool SendMail(MailData mailData)
        {
            try
            {
                using (var message = new MailMessage(_mailSettings.FromAddress, _mailSettings.ToAddress))
                {
                    message.Subject = mailData.EmailSubject;
                    message.Body = mailData.EmailBody;

                    using (var smtpClient = new SmtpClient(_mailSettings.SmtpServer, _mailSettings.SmtpPort))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = _credentials;
                        smtpClient.EnableSsl = true;

                        smtpClient.Send(message);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Email not sent: [{ex.Message}]");

                return false;
            }
        }
    }
}
