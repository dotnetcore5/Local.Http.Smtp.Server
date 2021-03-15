using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Local.Http.Email.Server.Http.Server
{
    public interface IEmailSender
    {
        Task SendEmail(NameValueCollection values);
    }

    internal class EmailSender : IEmailSender
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly IConfigurationRoot _config;

        public EmailSender(IConfigurationRoot config)
        {
            _config = config;
        }

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
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
            try
            {
                var httpResponse = await httpClient.PostAsync(_config["EmailEndpoint"], httpContent);
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                }
            }
            catch
            {
            }
        }
    }
}