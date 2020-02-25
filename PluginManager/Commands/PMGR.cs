using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;
using PluginManager.Helpers;
using System;

namespace PluginManager.Commands
{
    public class PMGR : ICommand
    {
        public string Name => "PMGR";

        public string Description => "Plugin Manager";

        public void Execute(CommandElements parameters, IConsole console)
        {
            Manager.HandleCommand(parameters, console);
        }
        private string GetProgressBar(int percent)
        {
            return new string('█', percent / 4).PadRight(25,' ');
        }
    }
}
