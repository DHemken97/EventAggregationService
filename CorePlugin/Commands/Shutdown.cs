using System;
using System.Linq;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;

namespace CorePlugin.Commands
{
    public class Shutdown:ICommand
    {
        public string Name { get=>"Shutdown"; }
        public string Description { get=>"Closes the Server"; }
        public void Execute(CommandElements parameters, IConsoleWriter console)
        {
            var exitCode = parameters?.Arguments.FirstOrDefault();
            Environment.Exit(int.TryParse(exitCode, out var code) ? code : 0);
        }

    }
}
