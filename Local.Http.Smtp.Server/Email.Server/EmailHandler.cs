using System;
using System.IO;

namespace Local.Http.Email.Server.Email.Server
{
    public interface IEmailHandler
    {
        void Read(StreamReader reader, StreamWriter writer);
    }

    internal class EmailHandler : IEmailHandler
    {
        private IContentParser _contentParser;

        public EmailHandler(IContentParser contentParser)
        {
            _contentParser = contentParser; ;
        }

        public void Read(StreamReader reader, StreamWriter writer)
        {
            while (reader != null)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;

                switch (line)
                {
                    case "DATA":
                        writer.WriteLine("354 Start input, end data with <CRLF>.<CRLF>");
                        var content = _contentParser.ParseEmail(reader, line);
                        string message = content.Item1;
                        Console.Error.WriteLine("Received ­ email with subject: {0} and message: {1}", content.Item2, message);
                        Console.Error.WriteLine("250 OK");
                        writer.WriteLine("250 OK");
                        break;

                    case "QUIT":
                        writer.WriteLine("250 OK");
                        reader = null;
                        break;

                    default:
                        writer.WriteLine("250 OK");
                        break;
                }
            }
        }
    }
}