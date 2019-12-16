using EAS_Development_Interfaces.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace CorePlugin.Models
{
    public class TelnetConsole : IConsole
    {
        private NetworkStream NetworkStream;
        public TelnetConsole(NetworkStream stream)
        {
            NetworkStream = stream;

        }
        public void Write(string value)
        {
            var msg = Encoding.ASCII.GetBytes(value);

            NetworkStream.Write(msg, 0, msg.Length);
        }
    }
}
