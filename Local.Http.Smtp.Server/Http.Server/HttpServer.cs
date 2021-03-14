using Local.Http.Email.Server.Common;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace Local.Http.Email.Server.Http.Server
{
    public interface IHttpServer
    {
        Task StartAsync();
    }

    internal class HttpServer : IHttpServer
    {
        private static HttpListener listener;
        private IHttpRequestHandler _httpRequestHandler;
        private readonly IConfigurationRoot _config;

        public HttpServer(IConfigurationRoot config, IHttpRequestHandler httpRequestHandler)
        {
            _config = config;
            _httpRequestHandler = httpRequestHandler;
            listener = new HttpListener();
            var url = _config["HttpServer:BaseAddress"] + ":" + _config["HttpServer:Port"] + "/";
            listener.Prefixes.Add(url);
            listener.Start();
            url.ShowOnConsole("Http");
        }

        public async Task StartAsync()
        {
            bool runServer = true;
            while (runServer)
            {
                await _httpRequestHandler.HandleAsync(listener);
            }
        }
    }
}