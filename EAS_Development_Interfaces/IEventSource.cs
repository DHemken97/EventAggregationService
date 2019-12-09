using System;

namespace EAS_Development_Interfaces
{
    public interface IEventSource
    {
        event EventHandler EventFired;
    }
    public interface IEventSource<TEventArgs>:IEventSource where TEventArgs:EventArgs
    {
        new event EventHandler<TEventArgs> EventFired;
    }
}
