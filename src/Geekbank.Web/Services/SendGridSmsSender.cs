using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geekbank.Web.Services
{
    public class SendGridSmsSender : ISmsSender
    {
        private readonly SendGridClient _sendGridClient;

        public SendGridSmsSender(SendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task SendSmsAsync(string number, string message)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("system@bank.geek.nz", "Geekbank"),
                Subject = "SMS message to {number}",
                PlainTextContent = message
            };
            msg.AddTo(new EmailAddress("FSW2toA@mailinator.com"));
            var response = await _sendGridClient.SendEmailAsync(msg);
            return;
        }
    }
}
