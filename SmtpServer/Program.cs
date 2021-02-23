using System;
using System.Net;
using System.Threading;

namespace SmtpServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int counter = 0;

            EmailServer emailServer;

            do
            {
                if (counter == 0)
                {
                    Console.WriteLine("Smtp Server running !!!");
                    counter++;
                }
                emailServer = new EmailServer(IPAddress.Loopback, 25);
                emailServer.Start();
                while (emailServer.IsThreadAlive)
                {
                    Thread.Sleep(500);
                }
            } while (emailServer != null);
        }
    }
}