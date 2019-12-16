using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;
using PluginManager.Models;
using System.Linq;

namespace PluginManager.Helpers
{
    public static class Manager
    {
        public static void HandleCommand(CommandElements parameters, IConsole console)
        {
            var cmd = parameters.ToFormattedString(true);
            switch (parameters.Arguments.FirstOrDefault().ToLower())
            {
                case "list":
                case "ls":
                    List(new CommandElements(cmd),console);
                    break;

            }
        }
        private static void List(CommandElements parameters, IConsole console)
        {
            if (parameters.Flags.Contains("a"))
                ListAll(console);
            else if (parameters.Flags.Contains("i"))
                ListInstalled(console);
            else console.Write("Please Specify -a for all or -i for installed\r\n");

        }

        private static void ListInstalled(IConsole console)
        {
            Configuration.Assemblies.ForEach(a =>
            {
                console.Write($"{a.GetName()}\r\n");
            });
        }

        private static void ListAll(IConsole console)
        {
            var results = HttpHelper.Get<GitDirectory[]>("https://api.github.com/repos/dhemken97/Plugins/contents/").ToList();
            results.ForEach(r => console.Write($"{r.name}\r\n"));
        }
    }
}
