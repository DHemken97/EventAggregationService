using System.Collections.Generic;
using Newtonsoft.Json;

namespace EAS_Development_Interfaces
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj);
        public static Tobj FromJson<Tobj>(this string str)
            => JsonConvert.DeserializeObject<Tobj>(str);
        
        public static Dictionary<string, Tobj> ToDictionary<Tobj>(this object obj)
            => obj.ToJson().FromJson<Dictionary<string, Tobj>>();

    }
}
