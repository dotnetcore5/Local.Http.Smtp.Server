using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Local.Http.Email.Server.Http.Server
{
    public interface IHttpRequestHandler
    {
        Task HandleAsync(HttpListener listener);
    }

    internal class HttpRequestHandler : IHttpRequestHandler
    {
        private int pageViews = 0;
        private int requestCount = 0;
        private readonly string htmlPost = File.ReadAllText(@"Websites/Website1/httpPost.html"), htmlGet = File.ReadAllText(@"Websites/Website1/httpGet.html");
        private readonly IRequestPayloadHandler _payloadHandler;

        public HttpRequestHandler(IRequestPayloadHandler payloadHandler)
        {
            _payloadHandler = payloadHandler;
        }

        public async Task HandleAsync(HttpListener listener)
        {
            var ctx = await listener.GetContextAsync();
            var httpRequest = ctx.Request;
            var httpResponse = ctx.Response;
            Console.WriteLine("http request received ...");
            Console.WriteLine("Request #: {0}", ++requestCount);
            Console.WriteLine(httpRequest.Url.ToString());
            Console.WriteLine(httpRequest.HttpMethod);
            Console.WriteLine("http request served ...");
            byte[] data;
            if (httpRequest.HttpMethod == "POST")
            {
                var postData = await _payloadHandler.ShowRequestPayload(httpRequest);
                await _payloadHandler.SendEmail(postData);
                data = Encoding.UTF8.GetBytes(string.Format(htmlGet, postData[0], postData[1], postData[2], postData[3]));
            }
            else
            {
                pageViews++;
                data = Encoding.UTF8.GetBytes(string.Format(htmlPost, pageViews));
            }
            httpResponse.ContentType = "text/html";
            httpResponse.ContentEncoding = Encoding.UTF8;
            httpResponse.ContentLength64 = data.LongLength;
            httpResponse.AddHeader("X-Response", "This is test header:");
            await httpResponse.OutputStream.WriteAsync(data, 0, data.Length);
            httpResponse.Close();
        }
    }
}