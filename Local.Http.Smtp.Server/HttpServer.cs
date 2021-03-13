using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Local.Http.Email.Server
{
    internal class HttpServer
    {
        public static HttpListener listener;
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string htmlResponse = File.ReadAllText(@"data/index.html");

        public HttpServer(string url)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            Console.WriteLine($"**************************************************************************************");
            Console.WriteLine($"**************************************************************************************");
            Console.WriteLine($"***                                                                                ***");
            Console.WriteLine($"***                Http server running on : {url}...              ***");
            Console.WriteLine($"***                                                                                ***");
            Console.WriteLine($"**************************************************************************************");
            Console.WriteLine($"**************************************************************************************");
        }

        public async Task StartAsync()
        {
            bool runServer = true;
            while (runServer)
            {
                var ctx = await listener.GetContextAsync();
                var req = ctx.Request;
                var resp = ctx.Response;
                Console.WriteLine("http request received ...");
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine();
                byte[] data;
                if (req.Url.AbsolutePath == "/favicon.ico")
                    pageViews += 1;
                string disableSubmit = !runServer ? "disabled" : "";

                if (req.HttpMethod == "POST")
                {
                    data = Encoding.UTF8.GetBytes(ShowRequestPayload(req));
                }
                else
                {
                    data = Encoding.UTF8.GetBytes(string.Format(htmlResponse, pageViews, disableSubmit));
                }
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;
                resp.AddHeader("X-Response", "This is test header:");

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        private static string ShowRequestPayload(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("No client data was sent with the request.");
                return string.Empty;
            }
            var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            if (request.ContentType != null)
            {
                Console.WriteLine("Client data content type {0}", request.ContentType);
            }
            Console.WriteLine("Client data content length {0}", request.ContentLength64);

            Console.WriteLine("Start of client data:");
            string requestPayLoad = reader.ReadToEnd();
            Console.WriteLine(requestPayLoad);
            Console.WriteLine("End of client data:");
            request.InputStream.Close();
            reader.Close();
            return requestPayLoad;
        }
    }
}