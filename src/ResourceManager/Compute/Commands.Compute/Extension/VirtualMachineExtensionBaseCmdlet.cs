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

using Microsoft.Azure.Management.Compute;

namespace Microsoft.Azure.Commands.Compute
{
    public abstract class VirtualMachineExtensionBaseCmdlet : ComputeClientBaseCmdlet
    {
        public IVirtualMachineExtensionOperations VirtualMachineExtensionClient
        {
            get
            {
                return ComputeClient.ComputeManagementClient.VirtualMachineExtensions;
            }
        }

        protected string GetLocationFromVm(string rgName, string vmName)
        {
            var vm = this.ComputeClient.ComputeManagementClient.VirtualMachines.Get(rgName, vmName);
            return vm.VirtualMachine.Location;
        }
    }
}
