using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CorePlugin.Models
{
    public class Client
    {
        public Client(TcpClient client)
        {
            this.client = client.Client;
            ConnectionCreatedAt = DateTime.Now;
            UpdateCommandStats(string.Empty);
        }

        public void UpdateCommandStats(string command)
        {
            LastCommand = command;
            LastCommandRunTime = DateTime.Now;
        }

        public Socket client { get; }
        public DateTime ConnectionCreatedAt { get; }
        public DateTime LastCommandRunTime { get; private set; }
        public string LastCommand { get; private set; }
    }
}
