using System.Collections.Generic;
using EAS_Development_Interfaces;

namespace CorePlugin.Commands
{
    public class Help:ICommand
    {
        public string Name
        {
            get => "Help";
        }
        public string Description { get => "Display commands"; }
        
        public string Execute(IEnumerable<string> parameters = null)
        {
            var commands = Configuration.Commands;
            var result = string.Empty;
            foreach (var command in commands)
            {
                result += $"{command.Name}  -  {command.Description}\r\n";
            }
            return result;
        }
    }
}
