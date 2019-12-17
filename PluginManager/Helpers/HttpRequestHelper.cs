using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Interfaces;

namespace PluginManager.Helpers
{
    public static class HttpRequestHelper
    {
        public static TType Get<TType>(string uri) {
     
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.UserAgent = "PluginManager";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            var msg = response.ToJson();
                            throw new Exception(msg);
                        }
                        var json = reader.ReadToEnd();
                        return json.FromJson<TType>();
                    }
            }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var msg = reader.ReadToEnd();
                        throw new Exception(msg);
                    }
            }
           

   

        }

    }

    public static class ext
    {
        public static void AddHeader(this HttpWebRequest request,string key,string val)
        {
            request.Headers.Add(key,val);
        }
    }
}
