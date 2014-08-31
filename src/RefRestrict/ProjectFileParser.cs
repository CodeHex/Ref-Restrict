using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefRestrict
{
    public static class ProjectFileParser
    {
        public static ProjectInfo References(string projectFilePath)
        {
            var doc = XDocument.Load(projectFilePath);
            var refs = doc.Descendants().Where(x => x.Name.LocalName == "Reference")
                                        .Select(x => x.Attribute("Include").Value)
                                        .ToList();

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
