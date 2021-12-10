using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using TestLogger.Library.Models.Extensions;
using TestLogger.Library.Models.Options;
using TestLogger.Library.Models.Services.Application;
using TestLogger.Library.Models.Services.Infrastructure;

namespace ConsoleTestLoggerMail
{
    class Program
    {
        static string environment = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT");

        static async Task Main(string[] args)
        {
            var host = AppStartup();
            var serviceProvider = host.Services;
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            IOptionsMonitor<SmtpOptions> smtpOptions = serviceProvider.GetService<IOptionsMonitor<SmtpOptions>>();
            int sendingRateLimit = smtpOptions.CurrentValue.SendingRateLimit;

            Console.WriteLine($"{DateTime.Now}: applicazione avviata");

            //funzione asincrona di test
            int r = await GetNumberAsync();
            Console.WriteLine($"Risultato operazione di test: {r}");

            //genero volutamente un evento critical nel log
            logger.LogCritical(new DriveNotFoundException(), "Errore critico simulato...");

            Console.WriteLine($"{DateTime.Now}: applicazione terminata");
        }

        static void ConfigurationSetup(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        static IHost AppStartup()
        {
            var builder = new ConfigurationBuilder();
            ConfigurationSetup(builder);

            var host = Host.CreateDefaultBuilder()
                        .ConfigureServices((context, services) =>
                        ConfigureServices(context, services)
                        ).Build();



            return host;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                var settings = context.Configuration.GetSection("EmailLoggerProvider").Get<EmailLoggerProviderOptions>();
                loggingBuilder.AddProvider<EmailLoggerProvider>(p => new EmailLoggerProvider(settings, p.GetService<IEmailSender>()));

                loggingBuilder.AddFilter<EmailLoggerProvider>((category, logLevel) =>
                {
                    //critical level only
                    if (logLevel == LogLevel.Critical)
                    {
                        return true;
                    }
                    return false;
                });
            });

          
            services.Configure<SmtpOptions>(context.Configuration.GetSection("Smtp"));
          
        }

        #region funzione asincrona di prova...
        private static async Task<int> GetNumberAsync()
        {
            int i = 1;
            return await Task.FromResult(i);

        }
        #endregion
    }
}
