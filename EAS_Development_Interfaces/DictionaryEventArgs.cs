using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAS_Development_Interfaces
{
    public class DictionaryEventArgs:EventArgs
    {
        public Dictionary<string, object> Values;
    }
}
