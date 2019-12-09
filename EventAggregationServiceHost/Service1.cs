using EAS_Development_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace EventAggregationServiceHost
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            GetAssemblies();
            LoadBootstrappers();
            LoadCommands();
            LoadConsumers();
            LoadSources();
            LoadServices();
            CreateBindings();
        }

        protected override void OnStop()
        {
            Configuration.Services.ForEach(service => service.Stop());
        }
        private void LoadBootstrappers()
        {
            Configuration.Bootstrappers = GetClassesOfType<IBootstrapper>();
            Configuration.Bootstrappers.ForEach(b => b.Init());
                
        }
        private void GetAssemblies()
        {

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

        }
        private List<TType> GetClassesOfType<TType>()
        {
            return Configuration
                .Assemblies
                .SelectMany(assembly => assembly.GetTypes().Where(type => typeof(TType).IsAssignableFrom(type)))
                .Select(c =>(TType) Activator.CreateInstance(c))
                .ToList();

        }
    }
}
