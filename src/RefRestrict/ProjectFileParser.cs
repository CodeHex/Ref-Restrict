using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Build.Evaluation;

namespace RefRestrict
{
    public static class ProjectFileParser
    {
        public static List<string> References(string projectFilePath)
        {
            var project = new Project(projectFilePath);
            return project.AllEvaluatedItems
                .Where(y => y.ItemType == "Reference")
                .Select(x => x.EvaluatedInclude).ToList();
        }
    }
}
