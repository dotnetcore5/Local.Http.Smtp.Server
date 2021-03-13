using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpServer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await ServerManager.Init();
        }
   }
}