using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Local.Http.Email.Server.Http.Server
{
    public interface IRequestPayloadHandler
    {
        Task<NameValueCollection> ShowRequestPayload(HttpListenerRequest request);

        Task SendEmail(NameValueCollection values);
    }

    internal class RequestPayloadHandler : IRequestPayloadHandler
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task SendEmail(NameValueCollection values)
        {
            var payload = new
            {
                to = "admin@localhost",
                from = values[1],
                subject = values[2],
                body = values[3]
            };
            var stringPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync("http://localhost:7071/api/email", httpContent);
            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
            }
        }

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