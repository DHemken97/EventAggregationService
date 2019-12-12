using System.Linq;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Models;

namespace CorePlugin.Commands
{
    public class Help:ICommand
    {
        public string Name
        {
            get => "Help";
        }
        public string Description { get => "Display commands"; }
        
        public string Execute(CommandElements parameters)
        {
            var commands = Configuration.Commands;
            var searchValue = parameters?.Arguments.FirstOrDefault();
            var result = string.Empty;
            foreach (var command in commands.Where(cmd => 
            string.IsNullOrWhiteSpace(searchValue) || 
            cmd.Name.Contains(searchValue) || 
            cmd.Description.Contains(searchValue)
            ))
            {
                result += $"{command.Name}  -  {command.Description}\r\n";
            }
            return result;
        }
    }
}
