using System.Collections.Generic;

namespace EAS_Development_Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        string Execute(IEnumerable<string> parameters);
    }
}
