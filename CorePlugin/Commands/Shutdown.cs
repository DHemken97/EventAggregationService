using System;
using System.Collections.Generic;
using System.Linq;
using EAS_Development_Interfaces;

namespace CorePlugin.Commands
{
    public class Shutdown:ICommand
    {
        public string Name { get=>"Shutdown"; }
        public string Description { get=>"Closes the Server"; }
        public string Execute(IEnumerable<string> parameters)
        {
            var exitCode = parameters?.FirstOrDefault();
            Environment.Exit(int.TryParse(exitCode, out var code) ? code : 0);
            return null;
        }
    }
}
