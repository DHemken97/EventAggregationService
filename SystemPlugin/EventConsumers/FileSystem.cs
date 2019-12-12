using System.Collections.Generic;
using System.IO;
using EAS_Development_Interfaces;

namespace SystemPlugin.EventConsumers
{
    public class FileSystem:IEventConsumer
    {
        public string Name { get=>"File_System"; }
        public string Description { get=> "Writes Text to a Specified File"; }
    

        public void HandleEvent(object Sender, DictionaryEventArgs args)
        {
            File.WriteAllText(args.Values["FileName"].ToString(),args.Values["Contents"].ToString());

        }

        public List<string> RequiredValues => new List<string>(){"FileName","Contents"};
    }


}
