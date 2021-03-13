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
        private const string url = "http://localhost:5000/";
        public static string startData = File.ReadAllText(@"data/start.html");
        public static string indexData = File.ReadAllText(@"data/index.html");

        public static void Init()
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Http server running on : {0}", url);
        }

        public static async Task Start()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();
                // Make sure we don't increment the page views counter if `favicon.ico` is requested
                if (req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                // Write the response info
                byte[] data;
                string disableSubmit = !runServer ? "disabled" : "";

                if ((req.HttpMethod == "POST"))
                {
                    // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                    if (req.Url.AbsolutePath == "/shutdown")
                    {
                        Console.WriteLine("Shutdown requested");
                        runServer = false;
                        disableSubmit = !runServer ? "disabled" : "";
                        data = Encoding.UTF8.GetBytes(string.Format(startData, pageViews, disableSubmit));
                    }
                    else if (req.Url.AbsolutePath == "/start")
                    {
                        Console.WriteLine("Start requested");
                        runServer = true;
                        disableSubmit = !runServer ? "disabled" : "";
                        data = Encoding.UTF8.GetBytes(string.Format(indexData, pageViews, disableSubmit));
                    }
                    else
                    {
                        var requestPayload = ShowRequestPayload(req);
                        data = Encoding.UTF8.GetBytes(requestPayload);
                    }
                }
                else
                {
                    data = Encoding.UTF8.GetBytes(string.Format(indexData, pageViews, disableSubmit));
                }
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;
                resp.AddHeader("XResponse", "This is test header:");

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        private static string ShowRequestPayload(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("No client data was sent with the request.");
                return default;
            }
            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            if (request.ContentType != null)
            {
                Console.WriteLine("Client data content type {0}", request.ContentType);
            }
            Console.WriteLine("Client data content length {0}", request.ContentLength64);

            Console.WriteLine("Start of client data:");
            // Convert the data to a string and display it on the console.
            string requestPayLoad = reader.ReadToEnd();
            Console.WriteLine(requestPayLoad);
            Console.WriteLine("End of client data:");
            body.Close();
            reader.Close();
            // If you are finished with the request, it should be closed also.
            return requestPayLoad;
        }

        public static void Stop()
        {
            listener.Close();
        }
    }
}