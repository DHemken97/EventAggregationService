using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EAS_Development_Interfaces
{
    public static class Configuration
    {
        public static List<IBootstrapper> Bootstrappers { get; set; }
        public static List<Assembly> Assemblies { get; set; }
        public static List<ICommand> Commands { get; set; }
        public static List<IEventConsumer> EventConsumers { get; set; }
        public static List<IEventSource> EventSources { get; set; }
        public static List<IService> Services { get; set; }
        public static List<Binding> Bindings { get; set; }
        private static string BaseDirectory { get; set; }


        public static void Clear()
        {
            Bootstrappers = new List<IBootstrapper>();
            Assemblies = new List<Assembly>();
            Commands = new List<ICommand>();
            EventConsumers = new List<IEventConsumer>();
            EventSources = new List<IEventSource>();
            Services = new List<IService>();
            Bindings = new List<Binding>();
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

        }

        public static void Reload()
        {
            Clear();
            Load(BaseDirectory);
        }
        private static void LoadBootstrappers()
        {
            Configuration.Bootstrappers = GetClassesOfType<IBootstrapper>();
            Configuration.Bootstrappers.ForEach(b => b.Init());

        }
        private static void GetAssemblies()
        {
            var files = Directory
                .GetFiles($@"{BaseDirectory}\Plugins")
                .Where(file => file.ToLower()
                    .EndsWith(".dll"))
                .ToList();

            Configuration.Assemblies = files.Select(Assembly.LoadFile).ToList();
        }


        private static void LoadCommands()
        {
            Configuration.Commands = GetClassesOfType<ICommand>();
        }
        private static void LoadConsumers()
        {
            Configuration.EventConsumers = GetClassesOfType<IEventConsumer>();
        }
        private static void LoadSources()
        {
            Configuration.EventSources = GetClassesOfType<IEventSource>();
        }
        private static void LoadServices()
        {
            Configuration.Services = GetClassesOfType<IService>();
            Configuration.Services.ForEach(s => s.Start());
        }
        private static void CreateBindings()
        {
            Configuration.Bindings = new List<Binding>();
            var files = Directory
                .GetFiles($@"{BaseDirectory}\Bindings")
                .Where(file => file.ToLower()
                    .EndsWith(".json"))
                .ToList();
            files.ForEach(file => Configuration.Bindings.Add(File.ReadAllText(file).FromJson<Binding>()));
            var result = Configuration.Bindings.All(b => b.Bind());
            if (!result) File.WriteAllText($@"{BaseDirectory}\BindingErrors.txt", "Failed To Bind one or more items");
            //  else File.WriteAllText($@"{BaseDirectory}\BindingErrors.txt", "No Errors");
        }

        private static List<TType> GetClassesOfType<TType>()
        {
            return Configuration
                .Assemblies
                .SelectMany(assembly => assembly.GetTypes().Where(type => typeof(TType).IsAssignableFrom(type)))
                .Select(c => (TType)Activator.CreateInstance(c))
                .ToList();
        }
    }
}
