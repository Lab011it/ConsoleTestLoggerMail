using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLogger.Library.Models.Options;
using TestLogger.Library.Models.Services.Infrastructure;

namespace TestLogger.Library.Models.Services.Application
{
    public class EmailLogger : ILogger
    {
        private readonly EmailLoggerProviderOptions _settings;
        private readonly string _name;
        private readonly IEmailSender _emailSender;

        public EmailLogger(EmailLoggerProviderOptions settings, string name, IEmailSender emailSender)
        {
            _settings = settings;
            _name = name;
            _emailSender = emailSender;
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => logLevel == LogLevel.Critical;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
         
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string exceptionDetails = "";
            if (formatter != null)
            {
                if (exception != null)
                {
                    exceptionDetails = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}] {exception.GetType()}: { exception.Message} - {exception.InnerException} - {exception.StackTrace}";
                }
            }

         
           foreach (var toEmail in _settings?.GetParsedToEmails())
           {
               _emailSender.SendEmail(_settings.Sender, toEmail, $"({logLevel}) {_settings.Subject}", exceptionDetails);
               // -> await _emailSender.SendEmailAsync(_settings.Sender, toEmail, $"({logLevel}) {_settings.Subject}", exceptionDetails) ???
           }
        }


    }
}
