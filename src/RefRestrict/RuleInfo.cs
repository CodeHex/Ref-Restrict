using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefRestrict
{
    public class RefRuleSet
    {
        public bool IsRuleSetFoundInConfig { get; set; }
        public List<RefRule> Rules { get; set; }

        public RefRuleSet(string configPath, string projectName)
        {
            var config = XDocument.Load(configPath);

            var rules = config.Root.Elements("rules")
                                   .FirstOrDefault(x => x.Attribute("project").Value == projectName);

            IsRuleSetFoundInConfig = rules != null;
            if (IsRuleSetFoundInConfig)
            {
                Rules = new List<RefRule>();
                Rules.AddRange(rules.Elements().Select(x => RefRule.CreateRuleFromElement(x)));
            }        
        }
    }


    public class RefRule
    {
        public RuleType Type { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; }

        public static RefRule CreateRuleFromElement(XElement element)
        {
            if (element.Name == "nolocalrefs" || element.Name == "onlylocalrefs")
            {
                var rule = new RefRule { Type = RuleType.OnlyLocalReferences };
                rule.Values = element.Elements("project").Select(x => x.Value).ToList();
                return rule;
            }

            var newRule = new RefRule { Value = element.Value, Type = RuleType.Unknown };
            if (element.Name == "include")
                newRule.Type = RuleType.Include;

            if (element.Name == "exclude")
                newRule.Type = RuleType.Exclude;

            return newRule;
        }
    }

    public enum RuleType
    {
        Include,
        Exclude,
        OnlyLocalReferences,
        Unknown
    }
}
