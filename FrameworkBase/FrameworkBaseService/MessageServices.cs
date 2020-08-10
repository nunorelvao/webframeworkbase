using System;
using System.Threading.Tasks;
using FrameworkBaseService.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System.Threading;
using MailKit.Security;
using System.Collections.Generic;

namespace FrameworkBaseService
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string emailTO, string subject, string message)
        {
            // Plug in your email service here to send an email.

            try
            {
                //From Address
                string FromAddress = "nunorelvao@hotmail.com";
                string FromAdressTitle = "MyWebFramework";
                //To Address
                string ToAddress = emailTO;
                string ToAdressTitle = "MyWebFramework";
                string Subject = subject;
                string BodyContent = message;

                //Smtp Server
                string SmtpServer = "smtp.live.com";
                //Smtp Port Number
                int SmtpPortNumber = 587;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
                mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
                mimeMessage.Subject = Subject;
                mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = BodyContent
                };

                // mimeMessage.Body = "<html><body><strong>This is a Mail Message</strong></body></html>";

                using (var client = new SmtpClient())
                {
                    client.Connect(SmtpServer, SmtpPortNumber, SecureSocketOptions.StartTls);
                    client.Authenticate("nunorelvao@hotmail.com", "qhoiwqpqxukidivl");
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}