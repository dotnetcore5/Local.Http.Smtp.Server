using System.Net;
using System.Threading.Tasks;

namespace Local.Http.Email.Server
{
    internal class ServerManager
    {
        public static async Task Init()
        {
            _ = Task.Run(async () =>
              {
                  await StartHttpServer();
              });

            await Task.Run(() =>
            {
                StartEmailServer();
            });
        }

        private static async Task StartHttpServer()
        {
            const string httpServerBaseAddress = "http://localhost:5000/";
            int counter = 0;
            HttpServer httpServer;
            do
            {
                if (counter == 0)
                {
                    counter++;
                }
                httpServer = new HttpServer(httpServerBaseAddress);

                await httpServer.StartAsync();
            } while (httpServer != null);
        }

        private static void StartEmailServer()
        {
            const string smtpServerName = "127.0.0.1";
            const int smtpPort = 25;
            int counter = 0;
            Server server;
            do
            {
                if (counter == 0)
                {
                    counter++;
                }
                var ipAddress = IPAddress.Parse(smtpServerName);
                server = new Server(ipAddress, smtpPort);

                server.Start();
            } while (server != null);
        }
    }
}