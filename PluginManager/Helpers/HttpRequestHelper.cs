using System.IO;
using System.Net;
using EAS_Development_Interfaces;

namespace PluginManager.Helpers
{
    public static class HttpRequestHelper
    {
        public static TType Get<TType>(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd().FromJson<TType>();
            }
        }
    }
}
