using System.Collections.Generic;
using System.Linq;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Models;

namespace CorePlugin.Commands
{
    public class ExecuteCommand:ICommand
    {
        public string Name { get=> "Execute"; }
        public string Description { get=> "Runs an event consumer method"; }
        public string Execute(CommandElements parameters)
        {
            var name = parameters?.Arguments.FirstOrDefault();
            var consumer = Configuration.EventConsumers.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            if (consumer == null) return $"Unknown Consumer \"{name}\"";

            var arguments = parameters.Arguments.ToList();
            
            var d = new Dictionary<string,object>();
            foreach (var keyValuePair in parameters.DoubleFlags)                
            {
                var correctedName =
                    consumer.RequiredValues.FirstOrDefault(c => c.ToLower() == keyValuePair.Key.ToLower());
                if (string.IsNullOrWhiteSpace(correctedName)) return $"Unknown Parameter {keyValuePair.Key}";
                d.Add(correctedName, keyValuePair.Value);
            }

            if (!consumer.RequiredValues.All(k => d.ContainsKey(k)))
            {
                return $"Command Requires the following values :{string.Join("\r\n",consumer.RequiredValues)}";
            }
            consumer?.HandleEvent(this,new DictionaryEventArgs(){Values = d});
            return $"Executed Event {consumer.Name}";
        }
    }
}
