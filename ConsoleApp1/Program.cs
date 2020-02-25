using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;
using PluginManager.Internal;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new Manager();
            var commandElements = new CommandElements("PMGR Install Core");
            var writer = new ConsoleWriter();
            manager.HandleRequest(commandElements,writer);
            Console.ReadLine();
        }
    }

    public class ConsoleWriter : IConsoleWriter
    {
        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
