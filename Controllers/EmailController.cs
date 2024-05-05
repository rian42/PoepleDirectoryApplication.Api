using Microsoft.AspNetCore.Mvc;
using PoepleDirectoryApplication.Api.Models;
using PoepleDirectoryApplication.Api.Providers;

namespace PoepleDirectoryApplication.Api.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {

        private readonly ILogger<EmailController> _logger;
        private readonly IEmailProvider _emailProvider;

        public EmailController(IEmailProvider emailProvider, ILogger<EmailController> logger)
        {
            _emailProvider = emailProvider;
            _logger = logger;
        }

        [HttpPost(Name = "send")]
        public IActionResult SendEmail(string subject, string body)
        {
            try
            {
                var mailData = new MailData
                {
                    EmailBody = body,
                    EmailSubject = subject,
                };

                if (_emailProvider.SendMail(mailData))
                    return Ok("Email sent successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                // TODO: create a custom exception class maybe
                throw new Exception(ex.Message);
            }
        }
    }
}