using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefRestrict
{
    public class RefRuleInfo
    {
        public List<RefRuleSet> ProjectRules {get;set;}

        public RefRuleInfo(string configPath)
        {
            var config = XDocument.Load(configPath);

            var rulesets = config.Nodes();


            ProjectRules = new List<RefRuleSet>();

        }


        public bool HasRules(string projectName)
        {
            return ProjectRules.Any(x => x.ProjectName == projectName);
        }
    }

    public class RefRuleSet
    {
        public string ProjectName { get; set; }
        public List<RefRule> Rules {get;set;}
    }

    public class RefRule
    {

    }
}
