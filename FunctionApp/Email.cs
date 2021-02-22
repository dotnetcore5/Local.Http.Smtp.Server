using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;

namespace FunctionApp
{
    public static class Email
    {
        [FunctionName("email")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "email")] HttpRequest req, ILogger log)
        {
            try
            {
                var smtpClient = new SmtpClient("localhost", 25);
                var message = new MailMessage("admin@localhost", "user@localhost", "This is a test email", "Hi, Please click on this email, Thanks");
                smtpClient.SendAsync(message, null);
            }
            catch (Exception ex)
            {
                throw;
            }
            return new OkObjectResult("good");
        }
    }
}