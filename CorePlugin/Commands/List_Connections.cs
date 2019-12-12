using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CorePlugin.Models;
using CorePlugin.Services;
using EAS_Development_Interfaces;

namespace CorePlugin.Commands
{
    public class List_Connections:ICommand
    {
        public string Name => "List_Connections";
        public string Description => "List Active Telnet Connections";
        public string Execute(IEnumerable<string> parameters)
        {
            var clients = TelnetServer.Clients;
            return string.Join("\r\n", clients.Select(client => $"{((IPEndPoint)client.client.RemoteEndPoint).Address}:{((IPEndPoint)client.client.RemoteEndPoint).Port} - Connection Details:{{\r\n{getDetails(client)}\r\n}}"));
        }

        private string getDetails(Client client)
        {
            var idleTime = (DateTime.Now - client.LastCommandRunTime).Minutes;
            var status = idleTime < 1 ? "Active Now" : $"Idle for {idleTime} Minutes";
            return $"Status:{status}\r\nLast Command {client.LastCommand}\r\n" +
                   $"Connected Since {client.ConnectionCreatedAt}\r\n";
        }
    }
}
