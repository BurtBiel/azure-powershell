// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.ResourceManager.Common;
using Microsoft.CLU;
using Microsoft.CLU.CommandModel;
using Microsoft.CLU.Common;
using Microsoft.WindowsAzure.Commands.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Azure.Commands
{
    public class EntryStub
    {
        public static int Main(string[] args)
        {
            // empty entry point
            Console.WriteLine("We made it");
            Console.ReadLine();

            string discriminators = GetDiscriminators(args);
            AzureRMCmdlet cmdLet = LoadCmdlet(discriminators);
            BindArguments(cmdLet, args);
            return ExecuteCmdlet(cmdLet, args);
        }

        private static string GetDiscriminators(string[] args)
        {
            return string.Join(" ", args.Take(2));
        }

        private static AzureRMCmdlet LoadCmdlet(string discriminators)
        {
            var assm = Assembly.Load(new AssemblyName(@"Microsoft.Azure.Commands.Profile"));
            foreach (Type type in assm.GetTypes())
            {
                foreach (var attr in type.GetTypeInfo().GetCustomAttributes<CliCommandAliasAttribute>())
                {
                    if (attr.CommandName.Equals(discriminators, StringComparison.OrdinalIgnoreCase))
                    {
                        return (AzureRMCmdlet)Activator.CreateInstance(type);
                    }
                }
            }
            return null;
        }

        private static void BindArguments(AzureRMCmdlet cmdLet, string[] args)
        {
            // TODO
        }

        private static int ExecuteCmdlet(AzureRMCmdlet cmdLet, string[] args)
        {
            CmdletCommandModel commandModel = new CmdletCommandModel();
            ConfigurationDictionary dict = new ConfigurationDictionary();
            dict.Add("RtPackage", "Microsoft.CLU.Commands");
            dict.Add("RtEntry", "Microsoft.CLU.CommandModel.CmdletCommandModel.Run");
            dict.Add("RtAssembly", "Microsoft.CLU.dll");
            dict.Add("Modules", "Microsoft.Azure.Commands.Profile");
            dict.Add("NounPrefix", "AzureRm");

            CLUEnvironment.Console = new ConsoleInputOutput(args);

            var assemblyLocation = typeof(Microsoft.CLU.Common.AssemblyResolver).GetTypeInfo().Assembly.Location;
            var rootPath = System.IO.Path.GetDirectoryName(assemblyLocation);
            CLUEnvironment.SetRootPaths(rootPath);

            commandModel.Run(dict, args);

            //CLURunspace runspace = new CLURunspace(InitialSessionState.CreateDefault());
            //runspace.Open();
            //CommandCollection commandCollection = new CommandCollection();
            //commandCollection.Add(new Command(string.Join(" ", args)));
            //CLUSyncPipeline pipeline = new CLUSyncPipeline(runspace, commandCollection);
            //pipeline.Invoke();
            return 0;
        }
    }
}
