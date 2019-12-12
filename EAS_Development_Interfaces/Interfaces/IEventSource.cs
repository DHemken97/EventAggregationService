using System;
using System.Collections.Generic;

namespace EAS_Development_Interfaces
{
    public interface IEventSource
    {
        string Name { get; }
        string Description { get; }
        event EventHandler<DictionaryEventArgs> EventFired;
        List<string> AvailableValues { get; }
        bool IsRunning { get; }
    }
}
