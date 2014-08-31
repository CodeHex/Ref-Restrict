using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefRestrict
{
    /// <summary>
    /// Defines a report encapsulating results of checking references
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Indiviudal entries corresponding to indvidual checks
        /// </summary>
        public List<ReportEntry> Entries { get; set; }

        public Report()
        {
            Entries = new List<ReportEntry>();
        }
    }

    /// <summary>
    /// Defines a single report entry encapsulated one check or test
    /// </summary>
    public class ReportEntry
    {
        /// <summary>
        /// The level of the result
        /// </summary>
        public ReportLevel Level { get; private set; }

        /// <summary>
        /// The message associated with the entry
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The rule that was used to genenrate the entry, or null if not applicable
        /// </summary>
        public RefRule Rule { get; private set; }
 
        public ReportEntry(ReportLevel level, string message, RefRule rule = null)
        {
            Level = level;
            Message = message;
            Rule = rule;
        }
    }
}
