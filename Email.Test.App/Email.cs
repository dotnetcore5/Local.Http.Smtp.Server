using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Email.Test.App
{
    public static class Email
    {
        [FunctionName("email")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "email")] HttpRequest req, ILogger log)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var emailData = JsonConvert.DeserializeObject<EmailData>(content); ;
                var smtpClient = new SmtpClient("localhost", 25);
                await smtpClient.SendMailAsync(
                    emailData.From,
                    emailData.To,
                    emailData.Subject,
                    emailData.Body
                    );

                return new OkObjectResult($"Email sent to {emailData.To}!!!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class EmailData
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}