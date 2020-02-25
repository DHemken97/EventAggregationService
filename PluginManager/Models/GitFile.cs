using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.Models
{
    class GitFile
    {
   
            public string sha { get; set; }
            public string node_id { get; set; }
            public int size { get; set; }
            public string url { get; set; }
            public string content { get; set; }
            public string encoding { get; set; }
        
    }
}
