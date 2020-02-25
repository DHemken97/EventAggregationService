using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePlugin.Commands
{
    public class CLS : ICommand
    {
        public string Name => "CLS";

        public string Description => "Clear Screen";

        public void Execute(CommandElements parameters, IConsole console)
        {
            console.Write(Convert.ToChar(12).ToString());
        }
    }
}
