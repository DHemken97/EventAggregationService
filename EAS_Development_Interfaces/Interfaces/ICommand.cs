using EAS_Development_Interfaces.Interfaces;
using EAS_Development_Interfaces.Models;

namespace EAS_Development_Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        void Execute(CommandElements parameters,IConsoleWriter console);
    }
}
