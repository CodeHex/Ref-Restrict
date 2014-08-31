using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefRestrict
{
    public class ProjectInfo
    {
        public string Name { get; private set; }
        public List<string> Refs { get; private set; }
        public List<string> ProjectRefs { get; private set; }

        public ProjectInfo(string name, List<string> refs, List<string> projectRefs)
        {
            Name = name;
            Refs = refs;
            ProjectRefs = projectRefs;
        }
    }
}
