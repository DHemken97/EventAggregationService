using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;

namespace CorePlugin.Commands
{
    public class Reload:ICommand
    {
        public string Name => "Reload";
        public string Description => "Reload Plugins And Bindings";
        public void Execute(CommandElements parameters, IConsoleWriter console)
        {
            console.Write("Reloading...");
            var result = Configuration.Reload();
            console.Write("\rReloading...Done\r\n");
            console.Write(result);

        }
    }
}
