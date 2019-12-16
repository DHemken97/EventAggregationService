using EAS_Development_Interfaces;
using System.IO;
using System.Net;

namespace PluginManager.Helpers
{
    public static class HttpHelper
    {
        public static TType Get<TType>(string URL)
        {
            string stringResponse;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                stringResponse = reader.ReadToEnd();
            }
            return stringResponse.FromJson<TType>();
        }
    }
}
