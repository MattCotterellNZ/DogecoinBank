using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Geekbank.Web.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient _sendGridClient;

        public SendGridEmailSender(SendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("system@bank.geek.nz", "Geekbank"),
                Subject = subject,
                PlainTextContent = message
            };
            msg.AddTo(new EmailAddress(email));
            var response = await _sendGridClient.SendEmailAsync(msg);
            return;
        }
    }
}
