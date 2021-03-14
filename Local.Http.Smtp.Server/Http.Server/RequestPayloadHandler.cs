using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Local.Http.Email.Server.Http.Server
{
    public interface IRequestPayloadHandler
    {
        Task<NameValueCollection> ShowRequestPayload(HttpListenerRequest request);
    }

    internal class RequestPayloadHandler : IRequestPayloadHandler
    {
        public async Task<NameValueCollection> ShowRequestPayload(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("No client data was sent with the request.");
                return default;
            }
            NameValueCollection postData;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                if (request.ContentType != null)
                {
                    Console.WriteLine("Client data content type {0}", request.ContentType);
                }
                Console.WriteLine("Client data content length {0}", request.ContentLength64);
                var requestPayLoad = await reader.ReadToEndAsync();
                postData = HttpUtility.ParseQueryString(requestPayLoad);
                Console.WriteLine(requestPayLoad);
                request.InputStream.Close();
            }
            return postData;
        }
    }
}