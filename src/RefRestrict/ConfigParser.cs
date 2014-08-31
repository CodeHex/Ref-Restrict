using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefRestrict
{
    /// <summary>
    /// Used to parse the config file to create rules and rulesets
    /// </summary>
    public static class ConfigParser
    {
        /// <summary>
        /// Reads the config file and generates a corresponding ruleset for the given project
        /// </summary>
        /// <param name="configPath">The path to the config file</param>
        /// <param name="projectName">The project to create the ruleset for</param>
        /// <returns>The ruleset for the project</returns>
        public static RefRuleSet GetRuleSetForProject(string configPath, string projectName)
        {
            var config = XDocument.Load(configPath);

            // Get the rules corresponding to the project name
            var rulesData = config.Root.Elements("rules")
                                  .FirstOrDefault(x => x.Attribute("project").Value == projectName);

            // Convert the rules from the config into RefRule objects
            List<RefRule> rules = null;
            if (rulesData != null)
            {
                rules = new List<RefRule>();
                rules.AddRange(rulesData.Elements().Select(x => GetRuleFromElement(x)));
            }   
            return new RefRuleSet(rules);
        }

        /// <summary>
        /// Creates the corresponding rule given a single element from the config representing a rule
        /// </summary>
        /// <param name="element">The XML element representing the rule</param>
        /// <returns>The corresponding rule</returns>
        private static RefRule GetRuleFromElement(XElement element)
        {
            // No local reference rules is considered the same as only local references but with no provided projects
            if (element.Name == "nolocalrefs" || element.Name == "onlylocalrefs")
            {
                var ruleRefs = element.Elements("project").Select(x => x.Value).ToList();
                return new MultiRefRule(RuleType.OnlyLocalReferences, ruleRefs);
            }

            if (element.Name == "include")
                return new SingleRefRule(RuleType.Include, element.Value);

            if (element.Name == "exclude")
                return new SingleRefRule(RuleType.Exclude, element.Value);

            return new RefRule(RuleType.Unknown);
        }
    }
}
