using System.Collections.Generic;
using System.Linq;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;
using PluginManager.Helpers;
using PluginManager.Models;

namespace PluginManager.Internal
{
    internal class Manager
    {
        private IConsoleWriter _consoleWriter;

        public void HandleRequest(CommandElements parameters, IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
            var command = parameters.Arguments.FirstOrDefault().ToLower();
            switch (command)
            {
                default:
                    consoleWriter.Write($"Unknown Parameter '{command}'");
                    break;

                case "ls" when parameters.Flags.Contains("A"):
                case "list" when parameters.Flags.Contains("A"):
                    ListAllPlugins();
                    break;
                case "ls" when parameters.Flags.Contains("I"):
                case "list" when parameters.Flags.Contains("I"):
                case "ls":
                case "list":
                    ListInstalledPlugins();
                    break;
            }

        }

        private void ListInstalledPlugins()
        {
            Configuration.Assemblies.ForEach(a => _consoleWriter.Write($"{a.FullName}\r\n"));
        }

        private void ListAllPlugins()
        {
            var plugins = HttpRequestHelper.Get<GitObject[]>("api.github.com/repos/dhemken97/plugins/contents").ToList();
            plugins.ForEach(a => _consoleWriter.Write($"{a.name}\r\n"));
        }
    }
}
