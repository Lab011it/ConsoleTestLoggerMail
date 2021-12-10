using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLogger.Library.Models.Extensions
{
   public static class ILoggingBuilderExtensions
    {
        public static ILoggingBuilder AddProvider<T>(this ILoggingBuilder builder, Func<IServiceProvider, T> factory)
        where T : class, ILoggerProvider
        {
            builder.Services.AddSingleton<ILoggerProvider, T>(factory);
            return builder;
        }
    }
}
