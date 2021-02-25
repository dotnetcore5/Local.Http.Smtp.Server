using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpServer
{
    internal class ServerManager
    {
        public static async Task Init()
        {
            StartSmtpServer();
            await Task.Run(() =>
            {
                StartHttpServer().GetAwaiter();
            });
        }

        private static async Task StartHttpServer()
        {
            HttpServer.Init();
            Console.WriteLine("Http Server running !!!");
            await HttpServer.Start();
        }

        private static void StartSmtpServer()
        {
            EmailServer emailServer;

            do
            {
                Console.WriteLine("Smtp Server running !!!");
                emailServer = new EmailServer(IPAddress.Loopback, 25);
                emailServer.Start();
            } while (emailServer != null);
        }
    }
}