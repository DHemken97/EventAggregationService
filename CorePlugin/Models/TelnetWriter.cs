using System.Net.Sockets;
using System.Text;
using EAS_Development_Interfaces.Interfaces;

namespace CorePlugin.Models
{
    public class TelnetWriter:IConsoleWriter
    {
        private TcpClient client;

        public TelnetWriter(TcpClient client)
        {
            this.client = client;
        }

        public void Write(string value)
        {
            var dataStream = client.GetStream();
            var msg = Encoding.ASCII.GetBytes(value);
            dataStream.Write(msg, 0, msg.Length);
        }
    }
}
