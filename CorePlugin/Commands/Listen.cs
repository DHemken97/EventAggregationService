using System;
using System.Linq;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;

namespace CorePlugin.Commands
{
    public class Listen:ICommand
    {
        public string Name { get=>"Listen"; }
        public string Description { get=>"Listens for a given trigger"; }
        public void Execute(CommandElements parameters, IConsoleWriter console)
        {
            console.Write(GetResult(parameters));
        }
        public string GetResult(CommandElements parameters)
        {
            result = null;
            var name = parameters.Arguments.FirstOrDefault()??string.Empty;
            var source = Configuration.EventSources.FirstOrDefault(e => e.Name == name);
            if (source == null) return $"Unable to locate Source \"{name}\"";
            source.EventFired += HandleFire;
            while (result == null)
            {
                System.Threading.Thread.Sleep(1000);
            }

            source.EventFired -= HandleFire;
            return result;
        }

        private void HandleFire(object sender, DictionaryEventArgs eventArgs)
        {
            result = $"Event Fired at {DateTime.Now.ToShortTimeString()}\r\nEvent Args:{eventArgs.ToJson()}\r\n";
        }

        private string result;
    }
}
