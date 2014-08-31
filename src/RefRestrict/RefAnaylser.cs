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
            errorMessage = "";
            if (rule.Type == RuleType.Include)
            {
                errorMessage = rule.Value + " is required to be referenced in project " + project.Name;
                return project.Refs.Contains(rule.Value) || project.ProjectRefs.Contains(rule.Value);
            }

            if (rule.Type == RuleType.Exclude)
            {
                errorMessage = rule.Value + " should not be a reference in project " + project.Name;
                return !project.Refs.Contains(rule.Value) && !project.ProjectRefs.Contains(rule.Value);
            }

            if (rule.Type == RuleType.OnlyLocalReferences)
            {
                var invalidProjRefs = project.ProjectRefs.Where(x => !rule.Values.Contains(x));
                if (invalidProjRefs.Any())
                {
                    errorMessage = project.Name + " contains invalid local project references, ";
                    errorMessage += String.Join(", ",invalidProjRefs);
                    return false;
                }

                var missingProfRefs = rule.Values.Where(x => !project.ProjectRefs.Contains(x)).ToList();
                if (missingProfRefs.Any())
                {
                    errorMessage = project.Name + " should contain the local references, ";
                    errorMessage += String.Join(", ", missingProfRefs);
                    return false;
                }

                return true;
            }

            errorMessage = "Rule was not recognised";
            return false;
        }
    }
}
