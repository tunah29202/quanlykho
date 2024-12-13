using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
namespace Helpers.Email
{
    public class EmailHelpers : IEmailHelpers
    {
        private readonly IConfiguration configuration;
        public EmailHelpers(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public bool SendEmailWithBody(string body, string emailTo, string subject)
        {
            var from = new MailAddress(configuration.GetValue<string>("MailDetail:MailFrom"));
            var to = new MailAddress(emailTo);
            var mail = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            SendEmail(mail);
            return true;
        }
        #region private
        private void SendEmail(MailMessage message)
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = configuration.GetValue<string>("MailDetail:Host"),
                Port = configuration.GetValue<int>("MailDetail:Port"),
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(configuration.GetValue<string>("MailDetail:UserName"), configuration.GetValue<string>("MailDetail:PassWord")),
                EnableSsl = true,
            };
            smtp.Send(message);
        }
        #endregion
    }
}
