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
            if (args.Count() < 1 || !File.Exists(args[0]))
            {
                Console.WriteLine("ERROR: Unable to load project file");
                return 2;
            }

            var refs = ProjectFileParser.References(args[0]);

            refs.ForEach(x => Console.WriteLine("ERROR: Detected ref " + x));
            return 67;
        }
    }
}
