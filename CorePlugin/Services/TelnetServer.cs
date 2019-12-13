using EAS_Development_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CorePlugin.Models;
using EAS_Development_Interfaces.Helpers;

namespace CorePlugin.Services
{
    public class TelnetServer : IService
    {
        public static List<Client> Clients { get; private set; }
        public void Start()
        {
            Clients = new List<Client>();
            for (var port = 9090; port <= 9091; port++)
            {
                RunServer(port);
            }
        }

        private TcpListener _server;
        private async void RunServer(int port)
        {
            string data;
            try
            {
                var localAddr = IPAddress.Parse("127.0.0.1");

                _server = new TcpListener(localAddr, port);

                _server.Start();

                var bytes = new byte[256];

                await Task.Run(() =>
                {
                    while (true)
                    {

                        var client = _server.AcceptTcpClient();
                        var listEntry = new Client(client);
                        Clients.Add(listEntry);
                        var stream = client.GetStream();

                        int i;

                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data = Encoding.ASCII.GetString(bytes, 0, i);


                            var reply = HandleCommand(listEntry,data);
                            var msg = Encoding.ASCII.GetBytes(reply);

                            stream.Write(msg, 0, msg.Length);
                        }

                        client.Close();
                        Clients.Remove(listEntry);
                    }

                });

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                throw;
            }
            finally
            {
                _server?.Stop();
            }


        }
        string HandleCommand(Client client,string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return string.Empty;
            Clients.Remove(client);
            client.UpdateCommandStats(command);
            Clients.Add(client);
            var commandElements =command.BreakdownCommand();
            var c = Configuration.Commands.FirstOrDefault(cmd => cmd.Name.ToLower() == commandElements.command.ToLower());
            var result = c?.Execute(commandElements) ?? Unknown();
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
