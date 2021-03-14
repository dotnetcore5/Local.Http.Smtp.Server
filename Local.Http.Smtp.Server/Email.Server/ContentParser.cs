using System;
using System.IO;
using System.Text;

namespace Local.Http.Email.Server.Email.Server
{
    internal class ContentParser
    {
        public Tuple<string, string> ParseEmail(StreamReader _reader, string line)
        {
            StringBuilder data = new StringBuilder();
            string subject = "";
            line = _reader.ReadLine();

            if (line != null && line != ".")
            {
                const string SUBJECT = "Subject: ";
                if (line.StartsWith(SUBJECT))
                {
                    subject = line[SUBJECT.Length..];
                }
                else
                {
                    data.AppendLine(line);
                }

                for (line = _reader.ReadLine(); line != null && line != "."; line = _reader.ReadLine())
                {
                    data.AppendLine(line);
                }
            }
            return new Tuple<string, string>(data.ToString(), subject);
        }
    }
}