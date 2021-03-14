using System;
using System.IO;

namespace Local.Http.Email.Server.Email.Server
{
    internal class EmailHandler
    {
        private ContentParser contentParser;

        public EmailHandler()
        {
            contentParser = new ContentParser();
        }

        public void Read(StreamReader _reader, StreamWriter _writer)
        {
            while (_reader != null)
            {
                string line = _reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;

                switch (line)
                {
                    case "DATA":
                        _writer.WriteLine("354 Start input, end data with <CRLF>.<CRLF>");
                        var content = contentParser.ParseEmail(_reader, line);
                        string message = content.Item1;
                        Console.Error.WriteLine("Received ­ email with subject: {0} and message: {1}", content.Item2, message);
                        Console.Error.WriteLine("250 OK");
                        _writer.WriteLine("250 OK");
                        break;

                    case "QUIT":
                        _writer.WriteLine("250 OK");
                        _reader = null;
                        break;

                    default:
                        _writer.WriteLine("250 OK");
                        break;
                }
            }
        }
    }
}