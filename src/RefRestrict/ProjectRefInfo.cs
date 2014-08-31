using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefRestrict
{
    public class ProjectRefInfo
    {
        public string Name { get; private set; }
        public List<string> References { get; private set; }

        public ProjectRefInfo(string name, List<string> refs)
        {
            Name = name;
            References = refs;
        }
    }
}
