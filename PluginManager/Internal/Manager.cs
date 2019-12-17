using System;
using System.Collections.Generic;
using System.Linq;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;
using PluginManager.Helpers;
using PluginManager.Models;

namespace PluginManager.Internal
{
    public class Manager
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
                case "install":
                    if (parameters.Arguments.Count > 2)
                        InstallPlugin(parameters.Arguments[1],parameters.Arguments[2]);
                    else if (parameters.Arguments.Count == 2)
                    InstallPlugin(parameters.Arguments[1]);
                    else
                    _consoleWriter.Write("Please Specify a plugin\r\n");

                    break;
            }

        }

        private void ListInstalledPlugins()
        {
            Configuration.Assemblies.ForEach(a => _consoleWriter?.Write($"{a.FullName}\r\n"));
        }

        public void ListAllPlugins()
        {
            try
            {
                var plugins = HttpRequestHelper.Get<GitObject[]>("https://api.github.com/repos/dhemken97/plugins/contents/").ToList();
                plugins.ForEach(a => _consoleWriter?.Write($"{a.name}\r\n"));
            }
            catch (Exception e)
            {
                _consoleWriter?.Write(e.Message+"\r\n");
            }

        }

        public void InstallPlugin(string name, string version = null)
        {
            var versions = HttpRequestHelper.Get<GitObject[]>($"https://api.github.com/repos/dhemken97/plugins/{name}/contents/").ToList();
            version = version ?? versions.OrderBy(v => v).Last().name;
            if (versions.All(v => v.name != version))
            {
                _consoleWriter.Write($"Unknown Version {version}");
                return;
            }

            var url = versions.FirstOrDefault(v => v.name == version).git_url;
            HttpRequestHelper.Get<byte[]>(url);

        }
    }
}
