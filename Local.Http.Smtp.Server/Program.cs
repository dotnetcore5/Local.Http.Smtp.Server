using Local.Http.Email.Server.Email.Server;
using Local.Http.Email.Server.Http.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Local.Http.Email.Server
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console(Serilog.Events.LogEventLevel.Debug).MinimumLevel.Debug().Enrich.FromLogContext().CreateLogger();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            }));
            serviceCollection.AddLogging();
            var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: true).Build();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<IServerManager, ServerManager>();
            serviceCollection.AddSingleton<IHttpRequestHandler, HttpRequestHandler>();
            serviceCollection.AddSingleton<IRequestPayloadHandler, RequestPayloadHandler>();
            serviceCollection.AddSingleton<IHttpServer, HttpServer>();
            serviceCollection.AddSingleton<IEmailServer, EmailServer>();
            serviceCollection.AddSingleton<IEmailHandler, EmailHandler>();
            serviceCollection.AddSingleton<IContentParser, ContentParser>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            try
            {
                await serviceProvider.GetService<IServerManager>().StartAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}