using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAS_Development_Interfaces;

namespace CorePlugin.Commands
{
    public class Listen:ICommand
    {
        public string Name { get=>"Listen"; }
        public string Description { get=>"Listens for a given trigger"; }
        public string Execute(IEnumerable<string> parameters)
        {
            result = null;
            var name = parameters.FirstOrDefault()??string.Empty;
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
