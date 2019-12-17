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
        public static List<Assembly> Assemblies { get; set; }
        private static List<Assembly> newAssemblies { get; set; }
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
            Assemblies = new List<Assembly>();

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
            Assemblies = newAssemblies;
            newAssemblies = new List<Assembly>();
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
                .GetFiles($@"{BaseDirectory}\Plugins")
                .Where(file => file.ToLower()
                    .EndsWith(".dll"))
                .ToList();

            newAssemblies = files.Select(Assembly.LoadFile).ToList();
            Assemblies.ForEach(a => newAssemblies.Remove(a));
          
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
            return newAssemblies
                .SelectMany(assembly => assembly.GetTypes().Where(type => typeof(TType).IsAssignableFrom(type)))
                .Select(c => (TType)Activator.CreateInstance(c))
                .ToList();
        }
    }
}
