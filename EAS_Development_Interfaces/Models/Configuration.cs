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
            
        }
        public static void Unload(string domainName)
        {
            var domain = Domains.FirstOrDefault(d => d.FriendlyName == domainName);
            Unload(domain);
        }
        public static void Unload(AppDomain domain)
        {

            var assembly = domain.GetAssemblies()[1];
            var filePath = assembly.CodeBase;
            Commands.Where(c => assembly == c.GetType().Assembly).ToList().ForEach(c => Commands.Remove(c));
            EventConsumers.Where(c => assembly == c.GetType().Assembly).ToList().ForEach(c => EventConsumers.Remove(c));
            EventSources.Where(c => assembly == c.GetType().Assembly).ToList().ForEach(c => EventSources.Remove(c));
            Services.Where(c => assembly == c.GetType().Assembly).ToList().ForEach(c =>{c.Stop();Services.Remove(c);});
            Bindings.Where(c => assembly == c.GetType().Assembly).ToList().ForEach(c => Bindings.Remove(c));
            Domains.Remove(domain);
            AppDomain.Unload(domain);
            GC.Collect(); // collects all unused memory
            GC.WaitForPendingFinalizers(); // wait until GC has finished its work
            GC.Collect();
            
             //File.Delete(filePath);
        }

        private static async void Delete(string path)
        {
            await Task.Run(() =>
            {

            });
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
            var existingDomains = newDomains.Where(d => Domains.Any(ed => ed.FriendlyName == d.FriendlyName)).ToList();
            existingDomains.ForEach(a => newDomains.Remove(a));

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

}
