using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Local.Http.Email.Server.Http.Server
{
    internal class HttpRequestHandler
    {
        public static int pageViews = 1;
        public static int requestCount = 0;
        public static string htmlPost = File.ReadAllText(@"data/httpPost.html"), htmlGet = File.ReadAllText(@"data/httpGet.html");
        private RequestPayloadHandler payloadHandler;

        public HttpRequestHandler()
        {
            payloadHandler = new RequestPayloadHandler();
        }

        public async Task Handle(HttpListener listener)
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
                var postData = payloadHandler.ShowRequestPayload(httpRequest);
                data = Encoding.UTF8.GetBytes(string.Format(htmlGet, "ajeet", "subject", "bosy"));
            }
            else
            {
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