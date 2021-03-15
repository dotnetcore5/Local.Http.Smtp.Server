using Local.Http.Email.Server.Common;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Local.Http.Email.Server.Http.Server
{
    public interface IHttpServer
    {
        Task StartAsync();
    }

    internal class HttpServer : IHttpServer
    {
        private readonly HttpListener listener;
        private readonly IHttpRequestHandler _httpRequestHandler;
        private readonly IConfigurationRoot _config;
        private readonly string _url;

        public HttpServer(IConfigurationRoot config, IHttpRequestHandler httpRequestHandler)
        {
            _config = config;
            _httpRequestHandler = httpRequestHandler;
            listener = new HttpListener();
            _url = _config["HttpServer:BaseAddress"] + ":" + _config["HttpServer:Port"] + "/";
            listener.Prefixes.Add(_url);
            listener.Start();
            _url.ShowOnConsole("Http");
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