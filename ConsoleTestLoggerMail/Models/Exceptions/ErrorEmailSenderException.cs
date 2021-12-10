using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogger.Library.Models.Exceptions
{
    public class ErrorEmailSenderException : Exception
    {
        public ErrorEmailSenderException(string recipient, Exception innerException) : base($"Errore durante l'invio del report all'utente {recipient} alle ore {DateTime.Now}", innerException)
        {

        }
    }
}
