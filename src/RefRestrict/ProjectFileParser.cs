using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefRestrict
{
    /// <summary>
    /// Used to parse .csproj Visual Studio project files
    /// </summary>
    public static class ProjectFileParser
    {
        /// <summary>
        /// Extracts reference information from project files
        /// </summary>
        /// <param name="projectFilePath">The path to the project file</param>
        /// <returns>A summary of the project</returns>
        public static ProjectInfo GetProjectInfo(string projectFilePath)
        {
            var doc = XDocument.Load(projectFilePath);

            // Determine global references
            var refs = doc.Descendants().Where(x => x.Name.LocalName == "Reference")
                                        .Select(x => x.Attribute("Include").Value)
                                        .ToList();

            // Determine local references
            var projrefs = doc.Descendants().Where(x => x.Name.LocalName == "ProjectReference")
                                        .Select(x => x.Elements().First(y => y.Name.LocalName == "Name").Value)
                                        .ToList();

            var projName = doc.Descendants()
                              .First(x => x.Name.LocalName == "AssemblyName")
                              .Value;

            return new ProjectInfo(projName, refs, projrefs);
        }
    }
}
