using System.Collections.Generic;
using CommandLine;

namespace CrmRequestInspector
{
    class CommandLineOptions
    {
        [Option('o', Required = true, HelpText ="List of operations to be executed")]
        public IEnumerable<string> Operations { get; set; }

        [Option('r', HelpText ="On error resume onto next operation on error")]
        public bool IsResumeOnError { get; set; }
    }
}
