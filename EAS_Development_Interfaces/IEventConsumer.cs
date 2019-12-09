using System;
using System.Collections.Generic;

namespace EAS_Development_Interfaces
{
    public interface IEventConsumer
    {
        string Name { get; }
        string Description { get; }
        void HandleEvent(object Sender, DictionaryEventArgs args);
        List<string> RequiredValues { get; }
    }
}
