using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmtpServer
{
    internal class HttpServer
    {
        public static HttpListener listener;
        public static int pageViews = 0;
        public static int requestCount = 0;

        private const string httpUrl = "http://localhost:5000/";
        public static string pageData = File.ReadAllText(@"data/index.html");

        public static void Init()
        {
            listener = new HttpListener();
            listener.Prefixes.Add(httpUrl);
            listener.Start();
        }

        public static async Task Start()
        {
            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                if (req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;
                resp.AddHeader("XResponse", "This is test header");

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }
    }
}