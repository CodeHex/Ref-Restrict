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
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RefRestrict.config");

            if (!CheckArguments(args) || !CheckConfig(configPath)) 
                return 1;

            var refinfo = ProjectFileParser.References(args[0]);
            var ruleinfo = new RefRuleInfo(configPath);

            if (!ruleinfo.HasRules(refinfo.Name))
            {
                Console.WriteLine("WARNING: Ref-Restrict : No reference rules defined for project " + refinfo.Name);
                return 0;
            }


            refinfo.References.ForEach(x => Console.WriteLine("ERROR: Detected ref " + x));
            return 67;
        }

        private static bool CheckArguments(string[] args)
        {
            var validArgs = args.Count() >= 1 && File.Exists(args[0]);
            if (!validArgs)
                Console.WriteLine("ERROR: Unable to load project file");
            return validArgs;
        }

        private static bool CheckConfig(string configPath)
        {
            var fileFound = File.Exists(configPath);
            if (!fileFound)
                Console.WriteLine("ERROR: Unable to load configuration file");
            return fileFound;
        }
    }
}
