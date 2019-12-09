using EAS_Development_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePlugin.Services
{
    public class TelnetServer : IService
    {
        public void Start()
        {
            throw new NotImplementedException();
            
        }
        public void HandleCommand(string command)
        {
            var splitCommand = command.Split(' ');
            var cmd = Configuration.Commands.FirstOrDefault(c => c.Name.ToLower() == splitCommand.FirstOrDefault().ToLower());
            var param = splitCommand.ToList();
                param.RemoveAt(0);
            var result = cmd?.Execute(param.ToArray())??Write("Unknown Command\r\n");
        }
        public string Write(string value)
        {

            return "";
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
