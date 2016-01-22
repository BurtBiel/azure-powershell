using System;
using System.Diagnostics;
using System.IO;
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

            var commands = new CommandIndexCollection(Directory.GetCurrentDirectory());

            var exePath = commands.GetBestMatch(new CommandDiscriminator(args));
            if(exePath == null)
            {
                // TODO: show help
                return 1;
            }

            var processStart = new ProcessStartInfo(exePath.FullName, string.Join(" ", args));
            var process = Process.Start(processStart);
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}
