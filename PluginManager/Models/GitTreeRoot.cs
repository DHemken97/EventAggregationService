using System.Collections.Generic;

namespace PluginManager.Models
{
    public class Tree
    {
        public string path { get; set; }
        public string mode { get; set; }
        public string type { get; set; }
        public string sha { get; set; }
        public string url { get; set; }
    }

    public class GitTreeRoot
    {
        public string sha { get; set; }
        public string url { get; set; }
        public List<Tree> tree { get; set; }
        public bool truncated { get; set; }
    }
}
