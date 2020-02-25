using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.ServiceProcess;
using EAS_Development_Interfaces;

namespace EventAggregationServiceHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            SetupErrorHandling();
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void SetupErrorHandling()
        {
            AppDomain.CurrentDomain.FirstChanceException += FirstChanceHandler;
        }

        private static void FirstChanceHandler(object sender, FirstChanceExceptionEventArgs e)
        {
            var plugin = e.Exception.Source;
            var dir = $@"{Configuration.BaseDirectory}\Errors\{plugin}\";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.AppendAllText($"{dir}{e.Exception.GetType().Name}.txt", $"{e.Exception.Message}\r\n{e.Exception.StackTrace}\r\n\r\n");
        }
    }
}
