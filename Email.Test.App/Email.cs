using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.IO;
using Newtonsoft.Json;

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new OkObjectResult("Email sent !!!");
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