using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace EAS_Development_Interfaces
{
    public static class Configuration
    {
        public static List<IBootstrapper> Bootstrappers { get; set; }
        public static List<AppDomain> Domains { get; set; }
        private static List<AppDomain> newDomains { get; set; }
        public static List<ICommand> Commands { get; set; }
        public static List<IEventConsumer> EventConsumers { get; set; }
        public static List<IEventSource> EventSources { get; set; }
        public static List<IService> Services { get; set; }
        public static List<Binding> Bindings { get; set; }
        public static string BaseDirectory { get; private set; }


        public static void Clear()
        {
            Bootstrappers = new List<IBootstrapper>();
            Commands = new List<ICommand>();
            EventConsumers = new List<IEventConsumer>();
            EventSources = new List<IEventSource>();
            Services = new List<IService>();
            Bindings = new List<Binding>();
            Domains?.ForEach(AppDomain.Unload);
            Domains = new List<AppDomain>();

        }

        public static void Load(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
            GetAssemblies();
            LoadBootstrappers();
            LoadCommands();
            LoadConsumers();
            LoadSources();
            LoadServices();
            CreateBindings();
            Domains.AddRange(newDomains);
            newDomains = new List<AppDomain>();
            new Dump_Config().Execute();
            
        }
        public static void Unload(string domainName)
        {
            var domain = Domains.FirstOrDefault(d => d.FriendlyName == domainName);
            Unload(domain);
        }
        public static void Unload(AppDomain domain)
        {
            var filePath = domain.GetAssemblies().FirstOrDefault().CodeBase;
            Commands.Where(c => domain.GetAssemblies().Contains(c.GetType().Assembly)).ToList().ForEach(c => Commands.Remove(c));
            EventConsumers.Where(c => domain.GetAssemblies().Contains(c.GetType().Assembly)).ToList().ForEach(c => EventConsumers.Remove(c));
            EventSources.Where(c => domain.GetAssemblies().Contains(c.GetType().Assembly)).ToList().ForEach(c => EventSources.Remove(c));
            Services.Where(c => domain.GetAssemblies().Contains(c.GetType().Assembly)).ToList().ForEach(c =>{c.Stop();Services.Remove(c);});
            Bindings.Where(c => domain.GetAssemblies().Contains(c.GetType().Assembly)).ToList().ForEach(c => Bindings.Remove(c));
            AppDomain.Unload(domain);
            Domains.Remove(domain);
          //  GC.Collect(); // collects all unused memory
          //  GC.WaitForPendingFinalizers(); // wait until GC has finished its work
           // GC.Collect();
            File.Delete(filePath);
        }

        private static AppDomain GetDomain(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var _appDomain = AppDomain.CreateDomain(name);
            var runtimePath = $@"{BaseDirectory}\{name}.dll";
            File.Copy(path, runtimePath);
            _appDomain.Load(AssemblyName.GetAssemblyName(runtimePath));
                return _appDomain;

        }

        public static string Reload()
        {
            Load(BaseDirectory);
            return string.Empty;

        }
        private static void LoadBootstrappers()
        {
            Bootstrappers = GetClassesOfType<IBootstrapper>();
            Bootstrappers.ForEach(b => b.Init());

        }
        private static void GetAssemblies()
        {


            var files = Directory
                .GetFiles($@"{BaseDirectory}\Plugins","*.dll")
                .ToList();

            newDomains = files.Select(GetDomain).ToList();
            Domains.ForEach(a => newDomains.Remove(a));

        }


        private static void LoadCommands()
        {
            Commands = GetClassesOfType<ICommand>();
        }
        private static void LoadConsumers()
        {
            EventConsumers = GetClassesOfType<IEventConsumer>();
        }
        private static void LoadSources()
        {
            EventSources = GetClassesOfType<IEventSource>();
        }
        private static void LoadServices()
        {
            var newServices = GetClassesOfType<IService>();
            newServices.ForEach(s =>
            {
                s.Start();
            });
            Services.AddRange(newServices);
        }
        private static void CreateBindings()
        {
            Bindings = new List<Binding>();
            var files = Directory
                .GetFiles($@"{BaseDirectory}\Bindings")
                .Where(file => file.ToLower()
                    .EndsWith(".json"))
                .ToList();
            files.ForEach(file => Bindings.Add(File.ReadAllText(file).FromJson<Binding>()));
            var result = Bindings.All(b => b.Bind());
            if (!result) File.WriteAllText($@"{BaseDirectory}\BindingErrors.txt", "Failed To Bind one or more items");
            //  else File.WriteAllText($@"{BaseDirectory}\BindingErrors.txt", "No Errors");
        }

        private static List<TType> GetClassesOfType<TType>()
        {
            return newDomains.SelectMany(d => d.GetAssemblies())
                .SelectMany(assembly => assembly.GetTypes().Where(type => typeof(TType).IsAssignableFrom(type)))
                .Select(c => (TType)Activator.CreateInstance(c))
                .ToList();
        }
    }


    public  class Dump_Config 
    {
        public string Name { get => "Dump_Config"; }
        public string Description { get => "Lists All Values in running config"; }
        public void Execute()
        {
            File.WriteAllText($@"{Configuration.BaseDirectory}\config.txt", GetValues());
        }
        public string GetValues()
        {
            return
             ListAssemblies() +
             ListBootstrappers() +
             ListServices() +
             ListCommands() +
             ListSources() +
             ListConsumers() +
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
            string result = $"Loaded {Configuration.Domains.Count} Assemblies\r\n";
            foreach (var assembly in Configuration.Domains)
            {
                result += $"{assembly.FriendlyName}\r\n";
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
                result += $"{eventSource.Name} {(eventSource.IsRunning ? "Started" : "Stopped")}   -   {eventSource.Description}\r\n";
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
