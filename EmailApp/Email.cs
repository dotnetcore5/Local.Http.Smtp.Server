using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace Email.Test.App
{
    public static class Email
    {
        [FunctionName("email")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "email")] HttpRequest req, ILogger log)
        {
            try
            {
                var smtpClient = new SmtpClient("localhost", 25);
                await smtpClient.SendMailAsync("admin@localhost", "user@localhost", "This is a test email", "Hi, Please click on this email, Thanks");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new OkObjectResult("Email sent !!!");
        }
    }
}