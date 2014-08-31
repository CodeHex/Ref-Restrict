using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefRestrict
{
    /// <summary>
    /// Defines a collections of rules
    /// </summary>
    public class RefRuleSet
    {
        /// <summary>
        /// The rules in the rule set
        /// </summary>
        public List<RefRule> Rules { get; set; }

        /// <summary>
        /// If the rules are null, we assume we could not create the rule set
        /// </summary>
        public bool IsRules { get { return Rules != null; } }

        public RefRuleSet(List<RefRule> rules)
        {
            Rules = rules;
        }
    }

    /// <summary>
    /// Defines a single rule with no references
    /// </summary>
    public class RefRule
    {
        public RuleType Type { get; set; }

        public RefRule(RuleType type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// Defines a single rule that used a single reference as its argument
    /// </summary>
    public class SingleRefRule : RefRule
    {
        public string Ref { get; set; }
        public SingleRefRule(RuleType type, string refdata) : base(type)
        {
            Ref = refdata;
        }
    }

    /// <summary>
    /// Defines a single rule that uses multiple references as its argument
    /// </summary>
    public class MultiRefRule : RefRule
    {
        public List<string> Refs { get; set; }
        public MultiRefRule(RuleType type, List<string> refs)
            : base(type)
        {
            Refs = refs;
        }
    }
}
