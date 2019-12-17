using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAS_Development_Interfaces.Interfaces;

namespace EAS_Development_Interfaces.Helpers
{
    public static class TaskSpinner
    {
        public static void RunTaskWithSpinner(IConsoleWriter writer,string message, Action action)
        {
            c = '/';
            var bgw = new BackgroundWorker();
            bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
            bgw.DoWork += delegate
            {
                action?.Invoke();
            };
            IsRunning = true;

            bgw.RunWorkerAsync();

            while (IsRunning)
            {
                writer.Write($"\r{message}{DateTime.Now.Second}");
            }

        }
        private static bool IsRunning { get; set; }
        private static char c;

        private static void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsRunning = false;
        }
    }
}
