using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EAS_Development_Interfaces
{
    
    public class Binding
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public Dictionary<string, string> Mappings;
        public bool Bind()
        {
            try
            {
                var source = Configuration.EventSources.FirstOrDefault(s => s.Name.ToLower() == Source.ToLower());
                var target = Configuration.EventConsumers.FirstOrDefault(s => s.Name.ToLower() == Target.ToLower());
                source.EventFired += (s, e) =>
                {
                    try
                    {
                        var sourceArguments = e;
                        var targetArguments = TransformArgs(sourceArguments);
                        target.HandleEvent(source, targetArguments);

                    }
                    catch (Exception exception)
                    {
                        throw;
                    }

                };
                return true;
            }
            catch (Exception )
            {
            }

            return false;
        }

        private DictionaryEventArgs TransformArgs(DictionaryEventArgs sourceArguments)
        {
            var resultDictionary = Mappings.ToJson().FromJson<Dictionary<string,string>>();
            foreach (var prop in Mappings)
            {
                foreach (var val in sourceArguments.Values)
                {
                    resultDictionary[prop.Key] = prop.Value.Replace($"{{{val.Key}}}", val.Value.ToString());
                }
            }

            return
                new DictionaryEventArgs
                {
                    Values = resultDictionary.ToJson().FromJson<Dictionary<string, object>>()
                };
        }
    }
}
