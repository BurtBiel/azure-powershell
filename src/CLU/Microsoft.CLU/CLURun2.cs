using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CLU.Help;
using System.Diagnostics;
using Microsoft.CLU.CommandLineParser;
using System.IO;

namespace Microsoft.CLU
{
    public static class CLURun2
    {
        public static CommandModelErrorCode Execute(string[] args)
        {
            const string pkgRoot = @"C:\azure-powershell\drop\clurun\win7-x64\pkgs";
            var command = CommandDispatchHelper.FindCommands(pkgRoot, args).FirstOrDefault();

            if (command == null)
            {
                Environment.SetEnvironmentVariable("PackagesRootPath", pkgRoot);
                UnixCommandLineParser parser = new UnixCommandLineParser();
                parser.PresentCommandHelp(args);
                return CommandModelErrorCode.CommandNotFound;
            }
            
            return Execute(command, args);
        }

        private static CommandModelErrorCode Execute(CommandDispatchHelper.CommandInfo command, string[] args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(GetExecutor(command), string.Join(" ", args));
            var process = Process.Start(startInfo);
            process.WaitForExit();
            return CommandModelErrorCode.Success;
        }

        private static string GetExecutor(CommandDispatchHelper.CommandInfo command)
        {
            return Path.Combine(command.Package.Path, command.Package.Version, command.Package.Name);//@"C:\azure-powershell\src\CLU\Microsoft.Azure.Commands.Profile\bin\Debug\publish\Microsoft.Azure.Commands.Profile.exe";//Path.Combine(command.Package.Path, command.Package.Version, "executor.cmd");
        }
    }
}
