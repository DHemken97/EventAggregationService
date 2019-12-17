using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;
using PluginManager.Internal;

namespace PluginManager.Commands
{
    public class PMGR_Command : ICommand
    {
        public string Name => "PMGR";

        public string Description => "Plugin Manager";

        public void Execute(CommandElements parameters, IConsoleWriter console)
        {
            new Manager().HandleRequest(parameters, console);
        }
    }
}
