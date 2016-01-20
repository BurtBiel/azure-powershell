using Microsoft.CLU.Common.Properties;
using System;
using System.Diagnostics;
using System.Linq;

namespace az
{
    /// <summary>
    /// Type responsible for parsing and bootstrapping command execution.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Microsoft.CLU.Run (az.exe) main entry point.
        /// </summary>
        /// <param name="args">The commandline arguments</param>
        public static int Main(string[] args)
        { 
            var debugClu = Environment.GetEnvironmentVariable("DebugCLU");
            if (!String.IsNullOrEmpty(debugClu))
            {
                System.Console.WriteLine("This is your chance to attach a debugger...");
                System.Console.ReadLine();
            }
            return (int) Microsoft.CLU.Run.az.Execute(args);

            
        }
    }
}
