using System;
using System.Net;
using System.Threading.Tasks;

namespace SmtpServer
{
    internal class ServerManager
    {
        private const string url = "http://localhost:5000/";

        public static async Task Init()
        {
            Task.Run(() =>
            {
                StartHttpServer();
            });
            await Task.Run(() =>
            {
                StartEmailServer();
            });
        }

        private static async Task StartHttpServer()
        {
            HttpServer.Init();

            // Handle requests
            await HttpServer.Start();

            // Close the listener
            await HttpServer.Start();
        }

        private static void StartEmailServer()
        {
            int counter = 0;
            Server server;
            do
            {
                if (counter == 0)
                {
                    counter++;
                }
                server = new Server(IPAddress.Loopback, 25);

                server.Start();
            } while (server != null);
        }
    }
}