using System;
using System.Net;
using System.Threading.Tasks;

namespace SmtpServer
{
    class ServerManager
    {
        const string url = "http://localhost:5000/";
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
        static async Task StartHttpServer()
        {
            HttpServer.Init();

            Console.WriteLine("Http server running on : {0}", url);
            // Handle requests
            await HttpServer.Start();

            // Close the listener
            await HttpServer.Start();
        }

        static void StartEmailServer()
        {
            int counter = 0;
            Server server;
            do
            {
                if (counter == 0)
                {
                    Console.WriteLine("Smtp Server running !!!");
                    counter++;
                }
                server = new Server(IPAddress.Loopback, 25);

                server.Start();
            } while (server != null);
        }
    }
}
