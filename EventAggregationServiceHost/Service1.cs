using EAS_Development_Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using EAS_Development_Interfaces.Helpers;
namespace EventAggregationServiceHost
{
    public partial class Service1 : ServiceBase
    {
        string BaseDirectory => new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                File.WriteAllText($@"{BaseDirectory}\Errors.txt", e.Message);
                throw;
            }

        }

        private void Start()
        {
            GetAssemblies();
            LoadBootstrappers();
            LoadCommands();
            LoadConsumers();
            LoadSources();
            LoadServices();
            CreateBindings();

            File.AppendAllText($@"{BaseDirectory}\BootConfig.txt", JsonExtensions.ToJson(Configuration.Assemblies,true));
            File.AppendAllText($@"{BaseDirectory}\BootConfig.txt", JsonExtensions.ToJson(Configuration.Commands, true));
            File.AppendAllText($@"{BaseDirectory}\BootConfig.txt", JsonExtensions.ToJson(Configuration.Services, true));

        }

        protected override void OnStop()
        {
            Configuration.Services.ForEach(service => service.Stop());
            Configuration.Bindings.ForEach(binding => File.WriteAllText($@"{BaseDirectory}\Bindings\{binding.Source}_{binding.Target}.json",binding.ToJson()));
        }
        private void LoadBootstrappers()
        {
            Configuration.Bootstrappers = GetClassesOfType<IBootstrapper>();
            Configuration.Bootstrappers.ForEach(b => b.Init());

        }
        private void GetAssemblies()
        {
            var files = Directory
                .GetFiles($@"{BaseDirectory}\Plugins")
                .Where(file => file.ToLower()
                    .EndsWith(".dll"))
                .ToList();

            Configuration.Assemblies = files.Select(Assembly.LoadFile).ToList();
        }
        private void LoadCommands()
        {
            Configuration.Commands = GetClassesOfType<ICommand>();
        }
        private void LoadConsumers()
        {
            Configuration.EventConsumers = GetClassesOfType<IEventConsumer>();
        }
        private void LoadSources()
        {
            Configuration.EventSources = GetClassesOfType<IEventSource>();
        }
        private void LoadServices()
        {
            Configuration.Services = GetClassesOfType<IService>();
            Configuration.Services.ForEach(s => s.Start());
        }
        private void CreateBindings()
        {
            Configuration.Bindings = new List<Binding>();
            var files = Directory
                .GetFiles($@"{BaseDirectory}\Bindings")
                .Where(file => file.ToLower()
                    .EndsWith(".json"))
                .ToList();
            files.ForEach(file => Configuration.Bindings.Add(File.ReadAllText(file).FromJson<Binding>()));
            //TestBinding();
            var result = Configuration.Bindings.All(b => b.Bind());
            if (!result) File.WriteAllText($@"{BaseDirectory}\BindingErrors.txt", "Failed To Bind one or more items");
          //  else File.WriteAllText($@"{BaseDirectory}\BindingErrors.txt", "No Errors");
        }

        private void TestBinding()
        {
            var binding = new Binding()
            {
                Source = Configuration.EventSources.First().Name,
                Target = Configuration.EventConsumers.First().Name,
                Mappings = new Dictionary<string, string>()
                {
                    {"FileName",@"C:\Users\dhemken\Projects\EAS\EventAggregationServiceHost\bin\Debug\Test.txt" },
                    {"Contents","It Is {Time}" }
                }
            };
            Configuration.Bindings.Add(binding);
        }
        private List<TType> GetClassesOfType<TType>()
        {
            return Configuration
                .Assemblies
                .SelectMany(assembly => assembly.GetTypes().Where(type => typeof(TType).IsAssignableFrom(type)))
                .Select(c => (TType)Activator.CreateInstance(c))
                .ToList();
        }
    }
}