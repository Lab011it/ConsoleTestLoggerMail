using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestLogger.Library.Models.Options;
using TestLogger.Library.Models.Exceptions;

namespace TestLogger.Library.Models.Services.Infrastructure
{
    /// <summary>
    /// Classe Custom di invio email
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly IOptionsMonitor<SmtpOptions> smtpOptionsMonitor;

        public EmailSender(IOptionsMonitor<SmtpOptions> smtpOptionsMonitor)
        {
            this.smtpOptionsMonitor = smtpOptionsMonitor;
        }
        /// <summary>
        /// Questo metodo invia email usando il componente opensource MailKit (non usare smtpClient di Microsoft perchè non più aggiornato)
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string sender,string recipient, string subject, string htmlMessage, string attachments="")
        {
            try
            {
                var options = this.smtpOptionsMonitor.CurrentValue;

                using var client = new SmtpClient();
                
                await client.ConnectAsync(options.Host, options.Port, options.Security);
                await client.AuthenticateAsync(options.Username, options.Password);

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(sender));
                message.To.Add(MailboxAddress.Parse(recipient));
                message.Subject = subject;
             
                if (attachments!="")
                {
                    var builder = new BodyBuilder();
                    builder.HtmlBody = htmlMessage;
                    builder.TextBody = htmlMessage.Replace("<br />", Environment.NewLine);
                    builder.Attachments.Add(attachments);
                    message.Body = builder.ToMessageBody();
                }
                else
                {
                    message.Body = new TextPart("html") { Text = htmlMessage };
                }
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new ErrorEmailSenderException(recipient, ex);
            }
        }

        public void SendEmail(string sender, string recipient, string subject, string htmlMessage, string attachments = "")
        {
            try
            {
                var options = this.smtpOptionsMonitor.CurrentValue;

                using var client = new SmtpClient();

                client.Connect(options.Host, options.Port, options.Security);
                client.Authenticate(options.Username, options.Password);

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(sender));
                message.To.Add(MailboxAddress.Parse(recipient));
                message.Subject = subject;

                if (attachments != "")
                {
                    var builder = new BodyBuilder();
                    builder.HtmlBody = htmlMessage;
                    builder.TextBody = htmlMessage.Replace("<br />", Environment.NewLine);
                    builder.Attachments.Add(attachments);
                    message.Body = builder.ToMessageBody();
                }
                else
                {
                    message.Body = new TextPart("html") { Text = htmlMessage };
                }
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new ErrorEmailSenderException(recipient, ex);
            }
        }
    }
}
