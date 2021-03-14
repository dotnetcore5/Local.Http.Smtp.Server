using System;
using System.IO;
using System.Net;

namespace Local.Http.Email.Server.Http.Server
{
    internal class RequestPayloadHandler
    {
        public string ShowRequestPayload(HttpListenerRequest request)
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