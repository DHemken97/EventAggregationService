using EAS_Development_Interfaces.Interfaces;
using System;

namespace EAS_Development_Interfaces.Models
{
    public class ConsoleWriter : IConsoleWriter
    {
        public event EventHandler OnWrite;
        public void Write(string value)
        {
            OnWrite?.Invoke(value, null);
        }
    }
}
