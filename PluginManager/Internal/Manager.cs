﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                var plugins = GetAllPlugins();
                plugins.ForEach(a => _consoleWriter?.Write($"{a.name}\r\n"));
            }
            catch (Exception e)
            {
                _consoleWriter?.Write(e.Message+"\r\n");
            }

        }

        private List<GitObject> GetAllPlugins()
        {
            return HttpRequestHelper.Get<GitObject[]>("https://api.github.com/repos/dhemken97/plugins/contents/")
                .ToList();

        }

        public void InstallPlugin(string name, string version = null)
        {
            var plugins = GetAllPlugins();
            var plugin = plugins.FirstOrDefault(p =>
            {
                var formattedName = p.name.ToLower().Replace("plugin", "");
                return formattedName == name.ToLower();
            });
            if (plugin == null)
            {
                _consoleWriter.Write($"Unknown Plugin {name}");
                return;
            }
            var versions = HttpRequestHelper.Get<GitTreeRoot>($"https://api.github.com/repos/dhemken97/plugins/git/trees/{plugin.sha}").tree;
            version = version ?? versions.OrderBy(v => v).Last().path;
            var tree = versions.FirstOrDefault(t => t.path == version);
            if (tree == null)
            {
                _consoleWriter.Write($"Unknown Version {version}");
                return;
            }

            var url = tree.url;
            var Folder = HttpRequestHelper.Get<GitTreeRoot>(url);
            var fileDetails = HttpRequestHelper.Get<GitTreeRoot>(Folder.tree.FirstOrDefault().url);
            var file = HttpRequestHelper.Get<GitFile>(fileDetails.url);
            var bytes = Convert.FromBase64String(file.content);
            File.WriteAllBytes($"{Configuration.BaseDirectory}/Plugins/{plugin.name}.dll",bytes);
            _consoleWriter.Write(Configuration.Reload());

        }
    }
}
