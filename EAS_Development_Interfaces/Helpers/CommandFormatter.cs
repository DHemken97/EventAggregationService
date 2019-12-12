using EAS_Development_Interfaces.Models;

namespace EAS_Development_Interfaces.Helpers
{
    public static class CommandFormatter
    {
        public static CommandElements BreakdownCommand(this string command)
        {
            return new CommandElements(command);
        }
    }
}
