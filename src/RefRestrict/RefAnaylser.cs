using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefRestrict
{
    /// <summary>
    /// Used to analyse references of projects
    /// </summary>
    public static class RefAnaylser
    {
        /// <summary>
        /// Checks a specific rule against a provided project
        /// </summary>
        /// <param name="project">The project to apply the rule to</param>
        /// <param name="rule">The rule to apply</param>
        /// <param name="errorMessage">The error message describing why the rule failed (null if it passed)</param>
        /// <returns>True if the rule passed, otherwise false</returns>
        private static bool CheckRule(ProjectInfo project, RefRule rule, out string errorMessage)
        {
            errorMessage = null;

            if (rule.Type == RuleType.Include)
            {
                var includeRule = rule as SingleRefRule;
                // Check if ref is included in either the local or global references
                var isRefIncluded = project.Refs.Contains(includeRule.Ref) || project.ProjectRefs.Contains(includeRule.Ref) || project.NugetRefs.Contains(includeRule.Ref);
                if (!isRefIncluded)
                    errorMessage = includeRule.Ref + " is required to be referenced in project " + project.Name;
                return isRefIncluded; 
            }

            if (rule.Type == RuleType.Exclude)
            {
                var excludeRule = rule as SingleRefRule;
                // Check if the ref is not in either the global or local references
                var isRefExcluded = !project.Refs.Contains(excludeRule.Ref) && !project.ProjectRefs.Contains(excludeRule.Ref) && !project.NugetRefs.Contains(excludeRule.Ref);
                if (!isRefExcluded)
                    errorMessage = excludeRule.Ref + " should not be a reference in project " + project.Name;
                return isRefExcluded;
            }

            if (rule.Type == RuleType.OnlyLocalReferences)
            {
                var localRule = rule as MultiRefRule;
                // First check if there are any local project references that are not on the rules included list
                var invalidProjRefs = project.ProjectRefs.Where(x => !localRule.Refs.Contains(x));
                if (invalidProjRefs.Any())
                {
                    errorMessage = project.Name + " contains invalid local project references, ";
                    errorMessage += String.Join(", ",invalidProjRefs);
                    return false;
                }

                // Secondly, check if there are any references on the rules included list that is not in the project
                var missingProfRefs = localRule.Refs.Where(x => !project.ProjectRefs.Contains(x)).ToList();
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

        /// <summary>
        /// Generates a report when analysing a projects ruleset against the project info 
        /// </summary>
        /// <param name="ruleset"></param>
        /// <param name="proj"></param>
        /// <returns>The generated report</returns>
        public static Report GenerateReport(RefRuleSet ruleset, ProjectInfo proj)
        {
            var report = new Report();

            // Check if we actually managed to find any rules, and if return a warning.
            if (!ruleset.IsRules)
            {
                report.Entries.Add(new ReportEntry(ReportLevel.Error, "Configured for " + proj.Name + " but no rules section found, please add rules section or uninstall nuget package from project"));
                return report;
            }

            // Check if we actually managed to find any rules but none have been set
            if (!ruleset.IsRulesSet)
            {
                report.Entries.Add(new ReportEntry(ReportLevel.Warning, "Could not find any rules for project " + proj.Name));
                return report;
            }

            // Loop through each rule in the ruleset and check if against the project. Generate a report entry
            // for every checked rule that failed
            foreach (var rule in ruleset.Rules)
            {
                string error;
                var passed = CheckRule(proj, rule, out error);
                if (!passed)
                    report.Entries.Add(new ReportEntry(ReportLevel.Error, error, rule));
            }
            return report;
        }
    }
}
