using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Local.Http.Email.Server
{
    public class Server : TcpListener
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private int _port;
        private IPAddress _localaddr;

        public Server(IPAddress localaddr, int port) : base(localaddr, port)
        {
            _localaddr = localaddr;
            _port = port;
        }

        new public void Start()
        {
            base.Start();

            Console.WriteLine($"**************************************************************************************");
            Console.WriteLine($"**************************************************************************************");
            Console.WriteLine($"***                                                                                ***");
            Console.WriteLine($"***                Smtp Server running on : {_localaddr.ToString()}:{_port}...                        ***");
            Console.WriteLine($"***                                                                                ***");
            Console.WriteLine($"**************************************************************************************");
            Console.WriteLine($"**************************************************************************************");

            _client = AcceptTcpClient();
            _client.ReceiveTimeout = 50000;
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream);
            _writer.NewLine = "\r\n";
            _writer.AutoFlush = true;

            RunTask();
        }

        protected void RunTask()
        {
            _writer.WriteLine("220 localhost -- Fake proxy server");

            try
            {
                while (_reader != null)
                {
                    string line = _reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) break;

                    switch (line)
                    {
                        case "DATA":
                            _writer.WriteLine("354 Start input, end data with <CRLF>.<CRLF>");
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

                            string message = data.ToString();
                            Console.Error.WriteLine("Received ­ email with subject: {0} and message: {1}", subject, message);
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
            catch (IOException ex)
            {
                Console.WriteLine("Connection lost." + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _client.Close();
                Stop();
            }
        }
    }
}