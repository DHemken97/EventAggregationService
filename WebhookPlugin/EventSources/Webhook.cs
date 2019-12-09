using System;
using System.Collections.Generic;
using EAS_Development_Interfaces;

namespace WebhookPlugin.EventSources
{
    public class Webhook:IEventSource
    {
        public string Name { get; }
        public string Description { get; }
        public event EventHandler<DictionaryEventArgs> EventFired;
        public List<string> AvailableValues=> new List<string>()
        {
            "AddressParam1",
            "AddressParam2",
            "AddressParam3",
            "BodyParam1",
            "BodyParam2",
            "BodyParam3",
            "EventName",
            "OccuredAt"

        };
        public bool IsRunning { get; }
    }
}
