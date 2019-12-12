using System;
using System.Net.Sockets;

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
