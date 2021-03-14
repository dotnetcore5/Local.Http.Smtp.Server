using Local.Http.Email.Server.Email.Server;
using System.Net;
using System.Threading.Tasks;
using Local.Http.Email.Server.Http.Server;

namespace Local.Http.Email.Server
{
    internal class ServerManager
    {
        public static async Task StartAsync()
        {
            _ = Task.Run(async () => await StartHttpServer());

            await Task.Run(() => StartEmailServer());
        }

        private static async Task StartHttpServer()
        {
            const string httpServerBaseAddress = "http://localhost:5000/";
            HttpServer httpServer;
            do
            {
                httpServer = new HttpServer(httpServerBaseAddress);
                await httpServer.StartAsync();
            } while (httpServer != null);
        }

        private static void StartEmailServer()
        {
            const string smtpServerName = "127.0.0.1";
            const int smtpPort = 25;
            EmailServer server;
            do
            {
                var ipAddress = IPAddress.Parse(smtpServerName);
                server = new EmailServer(ipAddress, smtpPort);
                server.StartEmail();
            } while (server != null);
        }
    }
}