using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginManager.Internal;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new Manager();
            manager.ListAllPlugins();

        }
    }
}
