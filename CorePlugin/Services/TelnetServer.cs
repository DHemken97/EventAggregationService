using EAS_Development_Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CorePlugin.Services
{
    public class TelnetServer : IService
    {
        public void Start()
        {
            RunServer();
        }

        private TcpListener _server;
        private async void RunServer()
        {
            string data;
            try
            {
                var port = 9090;
                var localAddr = IPAddress.Parse("127.0.0.1");

                _server = new TcpListener(localAddr, port);

                _server.Start();

                var bytes = new byte[256];

                await Task.Run(() =>
                {
                    while (true)
                    {

                        var client = _server.AcceptTcpClient();

                        var stream = client.GetStream();

                        int i;

                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data = Encoding.ASCII.GetString(bytes, 0, i);


                            var reply = HandleCommand(data);

                            var msg = Encoding.ASCII.GetBytes(reply);

                            stream.Write(msg, 0, msg.Length);
                        }

                        client.Close();
                    }

                });

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                _server?.Stop();
            }


        }
        string HandleCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return string.Empty;
            var splitCommand = command.Split(' ');
            var arguments = splitCommand.ToList();
            arguments.RemoveAt(0);
            var c = Configuration.Commands.FirstOrDefault(cmd => cmd.Name.ToLower() == splitCommand.FirstOrDefault().ToLower());
            var result = c?.Execute(arguments) ?? Unknown();
            return result;
        }
        string Unknown()
        {
            return "Unknown command\r\n";
        }
        public void Stop()
        {
            _server.Stop();
        }
    }
}
