using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAS_Development_Interfaces;

namespace CorePlugin.Commands
{
    public class ExecuteCommand:ICommand
    {
        public string Name { get=> "Execute"; }
        public string Description { get=> "Runs an event consumer method"; }
        public string Execute(IEnumerable<string> parameters)
        {
            var name = parameters.FirstOrDefault();
            var consumer = Configuration.EventConsumers.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            if (consumer == null) return $"Unknown Consumer \"{name}\"";

            var arguments = parameters.ToList();
            arguments.RemoveAt(0);
            
            var KeyValues = arguments.Select(p => p.Split(':'));
            var d = new Dictionary<string,object>();
            foreach (var keyValuePair in KeyValues)
            {
                var correctedName =
                    consumer.RequiredValues.FirstOrDefault(c => c.ToLower() == keyValuePair[0].ToLower());
                if (string.IsNullOrWhiteSpace(correctedName)) return $"Unknown Parameter {keyValuePair[0]}";
                d.Add(correctedName, keyValuePair[1]);
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
