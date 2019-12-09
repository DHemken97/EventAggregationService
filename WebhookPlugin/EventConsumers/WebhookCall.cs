using System.Collections.Generic;
using EAS_Development_Interfaces;

namespace WebhookPlugin.EventConsumers
{
    public class WebhookCall : IEventConsumer
    {
        public string Name { get; }
        public string Description { get; }
        public void HandleEvent(object Sender, DictionaryEventArgs args)
        {
        }

        public List<string> RequiredValues =>new List<string>()
        {
            "Address",
            "Method",
            "Body"
        };
    }
}
