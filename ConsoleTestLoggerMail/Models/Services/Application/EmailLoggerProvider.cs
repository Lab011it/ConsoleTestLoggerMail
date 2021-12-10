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
    public sealed class EmailLoggerProvider : ILoggerProvider
    {
        private readonly EmailLoggerProviderOptions _settings;
        private readonly IEmailSender _emailSender;

        public EmailLoggerProvider(EmailLoggerProviderOptions settings, IEmailSender emailSender)
        {
            _settings = settings;
            _emailSender = emailSender;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EmailLogger(_settings, categoryName, _emailSender);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
