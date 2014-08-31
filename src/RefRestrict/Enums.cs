using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefRestrict
{
    /// <summary>
    /// Defines the different types of available rules to restrict references
    /// </summary>
    public enum RuleType
    {
        [Description("Indicates that the project must be include the reference in the local or global references")]
        Include,
        [Description("Indicates that the project must be not include the reference in the local or global references")]
        Exclude,
        [Description("Indicates local references that should be in the project, but nothing else")]
        OnlyLocalReferences,
        [Description("Rule can not be identified")]
        Unknown
    }

    /// <summary>
    /// Defines the level that each report entry represents
    /// </summary>
    public enum ReportLevel
    {
        Info,
        Warning,
        Error
    }
}
