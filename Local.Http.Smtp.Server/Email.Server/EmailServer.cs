using Local.Http.Email.Server.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Local.Http.Email.Server.Email.Server
{
    public interface IEmailServer
    {
        void Start();
    }

    internal class EmailServer : IEmailServer
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private int _port;
        private string _localaddr;
        private IEmailHandler _emailHandler;
        private TcpListener tcpListener;

        public EmailServer(IConfigurationRoot config, IEmailHandler emailHandler)
        {
            _localaddr = config["SmtpServer:BaseAddress"];
            _port = int.Parse(config["SmtpServer:Port"]);
            tcpListener = new TcpListener(IPAddress.Parse(_localaddr), _port);
            _emailHandler = emailHandler;
        }

        public void Start()
        {
            tcpListener.Start();
            string url = $"{ _localaddr }:{ _port}";
            url.ShowOnConsole("Email");
            _client = tcpListener.AcceptTcpClient();
            _client.ReceiveTimeout = 50000;
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream);
            _writer.NewLine = "\r\n";
            _writer.AutoFlush = true;
            RunTask();
        }

        private void RunTask()
        {
            _writer.WriteLine("220 localhost -- Fake proxy server");

            try
            {
                _emailHandler.Read(_reader, _writer);
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
                tcpListener.Stop();
            }
        }
    }
}