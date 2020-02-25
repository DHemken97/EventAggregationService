using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAS_Development_Interfaces;

namespace PluginManager.Internal
{
    public static class Extensions
    {
        public static bool IsOneOf<TType>(this TType t,params TType[] p)
        {
            return p.Any(param => param.ToJson() == t.ToJson());
        }
    }
}
