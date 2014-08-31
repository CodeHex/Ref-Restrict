using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RefRestrict
{
    class Program
    {
        // The default name of the configuration file
        private const string DefaultConfigFileName = "RefRestrict.config.xml";

        /// <summary>
        /// Main entry point for RefRestict executable, will output Visual Studio compatible errors and warnings
        /// based on violated restrictions.
        /// </summary>
        /// <param name="args">
        /// arg0 - Path to the .csproj file of the project to anaylse
        /// arg1 (optional) - Path to config file containing the restrictions to impose
        /// </param>
        static void Main(string[] args)
        {
            string configFile;
            if (!CheckArguments(args, out configFile)) 
                return;

            // Get the project information about the references from the project file
            var projInfo = ProjectFileParser.GetProjectInfo(args[0]);

            // Get the rules associated with that project from the config file
            var ruleSet = ConfigParser.GetRuleSetForProject(configFile, projInfo.Name);

            // Generate a report by analysing the rules against the project information
            var results = RefAnaylser.GenerateReport(ruleSet, projInfo);

            OutputReportToConsole(results);
        }

        /// <summary>
        /// Checks that the arguments supplied are valid and returns a valid config file
        /// path to use
        /// </summary>
        /// <param name="args">The args passed into the application</param>
        /// <param name="configPath">The path of the config file to use</param>
        /// <returns>True, if everything is valid, otherwise false</returns>
        public static bool CheckArguments(string[] args, out string configPath)
        {
            configPath = null;
            // Check that the project file exists
            var validprojFile = args.Count() >= 1 && File.Exists(args[0]);
            if (!validprojFile)
            {
                Console.WriteLine("ERROR: Unable to load project file");
                return false;
            }

            configPath = GetConfigFile(args);
            if (configPath == null)
            {
                Console.WriteLine("ERROR: Unable to locate configuration file");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Determines the configuration file that should be used,
        /// </summary>
        /// <param name="args">The args passed to the application</param>
        /// <returns>The path to the config file, or null if one cannot be found</returns>
        public static string GetConfigFile(string[] args)
        {
            if ((args.Count() >= 2 && File.Exists(args[1])))
                return args[1];
               
            var localConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RefRestrict.config.xml");
            if (File.Exists(localConfigPath))
                return localConfigPath;

            return null;
        }

        /// <summary>
        /// Outputs the details of the report so it can be interpreted by Visual Studio
        /// </summary>
        /// <param name="report">The report to output</param>
        public static void OutputReportToConsole(Report report)
        {
            foreach (var entry in report.Entries)
            {
                string prefix = "";
                switch (entry.Level)
                {
                    case ReportLevel.Error:
                        prefix = "ERROR: ";
                        break;
                    case ReportLevel.Warning:
                        prefix = "WARNING: ";
                        break;
                }

                Console.WriteLine(prefix + "Ref-Restrict: " + entry.Message);
            }
        }
    }
}
