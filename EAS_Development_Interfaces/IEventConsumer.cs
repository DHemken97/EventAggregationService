using System;

namespace EAS_Development_Interfaces
{
    public interface IEventConsumer
    {
        string Name { get; }
        string Description { get; }
        void HandleEvent(object Sender, EventArgs args);

    }
    public interface IEventConsumer<TConsumerArgument> where TConsumerArgument: EventArgs, IConsumerArgument
    {
        void HandleEvent(object Sender, TConsumerArgument arguments);
    }
}
