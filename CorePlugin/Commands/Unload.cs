using System.Linq;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;

namespace CorePlugin.Commands
{
    public class Unload:ICommand
    {
        public string Name => "Unload";
        public string Description => "Unload a Plugin";
        public void Execute(CommandElements parameters, IConsoleWriter console)
        {
            Configuration.Unload(parameters.Arguments.FirstOrDefault());
        }
    }
}
