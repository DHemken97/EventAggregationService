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
           // Configuration.Clear();
            Configuration.Load(BaseDirectory);
 

            //File.AppendAllText($@"{BaseDirectory}\BootConfig.txt", JsonExtensions.ToJson(Configuration.Assemblies,true));
            //File.AppendAllText($@"{BaseDirectory}\BootConfig.txt", JsonExtensions.ToJson(Configuration.Commands, true));
            //File.AppendAllText($@"{BaseDirectory}\BootConfig.txt", JsonExtensions.ToJson(Configuration.Services, true));

        }

        protected override void OnStop()
        {
            Configuration.Services.ForEach(service => service.Stop());
            Configuration.Bindings.ForEach(binding => File.WriteAllText($@"{BaseDirectory}\Bindings\{binding.Source}_{binding.Target}.json",binding.ToJson()));
        }
       
    }
}