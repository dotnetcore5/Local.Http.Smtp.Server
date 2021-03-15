using Local.Http.Email.Server.Email.Server;
using System.Threading.Tasks;
using Local.Http.Email.Server.Http.Server;
using System.Diagnostics;

namespace Local.Http.Email.Server
{
    public interface IServerManager
    {
        Task StartAsync();
    }

    internal class ServerManager : IServerManager
    {
        private readonly IHttpServer _httpServer;
        private readonly IEmailServer _emailServer;

        public ServerManager(IHttpServer httpServer, IEmailServer emailServer)
        {
            _httpServer = httpServer;
            _emailServer = emailServer;
        }

        public async Task StartAsync()
        {
            _ = Task.Run(async () =>
             {
                 do
                 {
                     await _httpServer.StartAsync();
                 } while (_httpServer != null);
             });

            await Task.Run(() =>
            {
                do
                {
                    _emailServer.Start();
                } while (_emailServer != null);
            });
        }
    }
}