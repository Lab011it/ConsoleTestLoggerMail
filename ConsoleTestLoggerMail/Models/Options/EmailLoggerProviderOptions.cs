using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogger.Library.Models.Options
{
    public class EmailLoggerProviderOptions
    {
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string ToEmails { get; set; }
        public string[] GetParsedToEmails() => !string.IsNullOrEmpty(ToEmails) ? ToEmails.Split(";", StringSplitOptions.RemoveEmptyEntries) : new string[] { };
    }
}
