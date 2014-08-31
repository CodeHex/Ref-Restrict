using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefRestrict
{
    public static class RefAnaylser
    {
        public static bool CheckRule(ProjectInfo project, RefRule rule, out string errorMessage)
        {
            if (rule.Type == RuleType.Include)
            {
                errorMessage = rule.Value + " is required to be reference in project " + project.Name;
                return project.Refs.Contains(rule.Value) || project.ProjectRefs.Contains(rule.Value);
            }

            if (rule.Type == RuleType.Exclude)
            {
                errorMessage = rule.Value + " should not be a reference in project " + project.Name;
                return !project.Refs.Contains(rule.Value) && !project.ProjectRefs.Contains(rule.Value);
            }

            if (rule.Type == RuleType.NoProjectReference)
            {
                errorMessage = project.Name + " should not contain any other local project references";
                return !project.ProjectRefs.Any();
            }

            errorMessage = "Rule was not recognised";
            return false;
        }
    }
}
