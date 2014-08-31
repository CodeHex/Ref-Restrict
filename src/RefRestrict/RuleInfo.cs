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

        public static RefRule CreateRuleFromElement(XElement elememt)
        {
            if (elememt.Name == "norefs")
                return new RefRule { Type = RuleType.NoProjectReference };

            var newRule = new RefRule { Value = elememt.Value, Type = RuleType.Unknown };
            if (elememt.Name == "include")
                newRule.Type = RuleType.Include;

            if (elememt.Name == "exclude")
                newRule.Type = RuleType.Exclude;

            return newRule;
        }
    }

    public enum RuleType
    {
        Include,
        Exclude,
        NoProjectReference,
        Unknown
    }
}
