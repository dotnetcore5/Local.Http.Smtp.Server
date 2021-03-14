using Local.Http.Email.Server.Common;
using System.Net;
using System.Threading.Tasks;

namespace Local.Http.Email.Server.Http.Server
{
    internal class HttpServer
    {
        private static HttpListener listener;
        private HttpRequestHandler requestHandler;

        public HttpServer(string url)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            requestHandler = new HttpRequestHandler();
            url.ShowOnConsole("Http");
        }

        public async Task StartAsync()
        {
            bool runServer = true;
            while (runServer)
            {
                await requestHandler.Handle(listener);
            }
        }
    }
}