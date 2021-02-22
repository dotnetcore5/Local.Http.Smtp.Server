using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace SmtpServer
{
    public class EmailServer : TcpListener
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread thread = null;

        public bool IsThreadAlive
        {
            get { return thread.IsAlive; }
        }

        public EmailServer(IPAddress localaddr, int port) : base(localaddr, port)
        {
        }

        new public void Start()
        {
            base.Start();

            client = AcceptTcpClient();
            client.ReceiveTimeout = 5000;
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.NewLine = "\r\n";
            writer.AutoFlush = true;

            thread = new Thread(new ThreadStart(RunThread));
            thread.Start();
        }

        protected void RunThread()
        {
            writer.WriteLine("220 localhost -- Fake proxy server");

            try
            {
                while (reader != null)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) break;

                    Console.Error.WriteLine("Read line {0}", line);

                    switch (line)
                    {
                        case "DATA":
                            writer.WriteLine("354 Start input, end data with <CRLF>.<CRLF>");
                            StringBuilder data = new StringBuilder();
                            string subject = "";
                            line = reader.ReadLine();

                            if (line != null && line != ".")
                            {
                                const string SUBJECT = "Subject: ";
                                if (line.StartsWith(SUBJECT))
                                {
                                    subject = line.Substring(SUBJECT.Length);
                                }
                                else
                                {
                                    data.AppendLine(line);
                                }

                                for (line = reader.ReadLine(); line != null && line != "."; line = reader.ReadLine())
                                {
                                    data.AppendLine(line);
                                }
                            }

                            string message = data.ToString();
                            Console.Error.WriteLine("Received ­ email with subject: {0} and message: {1}", subject, message);
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
            catch (IOException ex)
            {
                Console.WriteLine("Connection lost.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                client.Close();
                Stop();
            }
        }
    }
}