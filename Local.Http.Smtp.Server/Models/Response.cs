using System;
using System.Collections.Generic;
using System.Text;

namespace Local.Http.Email.Server.Models
{
    internal class Response
    {
        public string Fullname { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}