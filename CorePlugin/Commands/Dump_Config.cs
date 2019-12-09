using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAS_Development_Interfaces;

namespace CorePlugin.Commands
{
    public class Dump_Config:ICommand
    {
        public string Name { get=>"Dump_Config"; }
        public string Description { get=> "Lists All Values in running config"; }
        public string Execute(IEnumerable<string> parameters)
        {  return 
            ListAssemblies()+
            ListBootstrappers()+
            ListServices()+
            ListCommands()+
            ListSources()+
            ListConsumers()+
            ListBindings();

        }

        private string ListBindings()
        {
            string result = $"Created {Configuration.Bindings.Count} Bindings\r\n";
            foreach (var binding in Configuration.Bindings)
            {
                result += $"{binding.Source} -> {binding.Target}\r\n";
            }

            return result;
        }


        private string ListAssemblies()
        {
            string result = $"Loaded {Configuration.Assemblies.Count} Assemblies\r\n";
            foreach (var assembly in Configuration.Assemblies)
            {
                result += $"{assembly.FullName}\r\n";
            }

            return result;
        }
        private string ListBootstrappers()
        {
            return $"Loaded {Configuration.Bootstrappers.Count} Bootstrappers\r\n";
        }

        private string ListServices()
        {
            return $"Loaded {Configuration.Services.Count} Services\r\n";
        }
        private string ListCommands()
        {
            string result = $"Loaded {Configuration.Commands.Count} Commands\r\n";
            foreach (var command in Configuration.Commands)
            {
                result += $"{command.Name}   -   {command.Description}\r\n";
            }

            return result;
        }
        private string ListSources()
        {
            string result = $"Loaded {Configuration.EventSources.Count} Event Sources\r\n";
            foreach (var eventSource in Configuration.EventSources)
            {
                result += $"{eventSource.Name} {(eventSource.IsRunning?"Started":"Stopped")}   -   {eventSource.Description}\r\n";
            }

            return result;
        }
        private string ListConsumers()
        {
            string result = $"Loaded {Configuration.EventConsumers.Count} Event Consumers\r\n";
            foreach (var eventConsumer in Configuration.EventConsumers)
            {
                result += $"{eventConsumer.Name}   -   {eventConsumer.Description}\r\n";
            }

            return result;
        }
    }
}
