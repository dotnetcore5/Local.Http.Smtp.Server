using Local.Http.Email.Server.Common;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Local.Http.Email.Server.Email.Server
{
    public class EmailServer : TcpListener
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private int _port;
        private IPAddress _localaddr;
        private EmailHandler _emailContentReader;

        public EmailServer(IPAddress localaddr, int port) : base(localaddr, port)
        {
            _localaddr = localaddr;
            _port = port;
        }

        public void StartEmail()
        {
            Start();
            string url = $"{ _localaddr }:{ _port}";
            url.ShowOnConsole("Email");
            _client = AcceptTcpClient();
            _client.ReceiveTimeout = 50000;
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream);
            _writer.NewLine = "\r\n";
            _writer.AutoFlush = true;
            _emailContentReader = new EmailHandler();
            RunTask();
        }

        private void RunTask()
        {
            _writer.WriteLine("220 localhost -- Fake proxy server");

            try
            {
                _emailContentReader.Read(_reader, _writer);
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