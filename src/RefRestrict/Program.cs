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
        static int Main(string[] args)
        {
            string configFile;
            if (!CheckArguments(args, out configFile)) 
                return 1;

            var projInfo = ProjectFileParser.References(args[0]);
            var ruleSet = new RefRuleSet(configFile, projInfo.Name);

            if (!ruleSet.IsRuleSetFoundInConfig)
            {
                Console.WriteLine("WARNING: Ref-Restrict : No reference rules defined for project " + projInfo.Name);
                return 0;
            }
            else
            {
                var allRulesOk = true;
                foreach (var rule in ruleSet.Rules)
                {
                    string err;
                    var passed = RefAnaylser.CheckRule(projInfo, rule, out err);
                    if (!passed)
                    {
                        allRulesOk = false;
                        Console.WriteLine("ERROR: " + err);
                    }
                }
                if (!allRulesOk)
                    return 1;
            }
            return 0;
        }

        private static bool CheckArguments(string[] args, out string configPath)
        {
            configPath = null;
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

        private static string GetConfigFile(string[] args)
        {
            if ((args.Count() >= 2 && File.Exists(args[1])))
                return args[1];
               
            var localConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RefRestrict.config");
            if (File.Exists(localConfigPath))
                return localConfigPath;

            return null;
        }
    }
}
