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

            if(args == null || args.Length == 0)
            {
                Console.WriteLine($"TODO: show general help");
                return 1;
            }

            var commands = new CommandIndexCollection(Directory.GetCurrentDirectory(), args);
            var discriminators = new CommandDiscriminator(args);
            var exePath = commands.GetBestMatchPath(ref discriminators);

            if (exePath == null)
            {
                Console.WriteLine($"TODO: show help for {discriminators}");
                return 1;
            }

            var processStart = new ProcessStartInfo(exePath, string.Join(" ", args.Skip(discriminators.Args.Length)));
            var process = Process.Start(processStart);
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}
