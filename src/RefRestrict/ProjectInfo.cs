using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefRestrict
{
    /// <summary>
    /// Defines a summary of a Visual Studio project
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The global references of the project, (system references)
        /// </summary>
        public List<string> Refs { get; private set; }

        /// <summary>
        /// The local references of the project (references to other projects in the same solution)
        /// </summary>
        public List<string> ProjectRefs { get; private set; }

        public ProjectInfo(string name, List<string> refs, List<string> projectRefs)
        {
            Name = name;
            Refs = refs;
            ProjectRefs = projectRefs;
        }
    }
}
