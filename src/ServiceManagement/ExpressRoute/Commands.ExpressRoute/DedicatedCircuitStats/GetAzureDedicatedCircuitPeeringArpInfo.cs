﻿// ----------------------------------------------------------------------------------
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


using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.WindowsAzure.Management.ExpressRoute.Models;
using System;

namespace Microsoft.WindowsAzure.Commands.ExpressRoute
{
    [Cmdlet(VerbsCommon.Get, "AzureDedicatedCircuitPeeringArpInfo"), OutputType(typeof(string), typeof(IEnumerable<string>))]
    public class GetAzureDedicatedCircuitPeeringArpInfoCommand : ExpressRouteBaseCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Service Key representing the Dedicated Circuit")]
        public Guid ServiceKey { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, 
            HelpMessage = "Bgp Peering Access Type: Public or Private or Microsoft")]
        public BgpPeeringAccessType AccessType { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true,
            HelpMessage = "Device Path: Primary or Secondary")]
        public DevicePath BgpPeeringDevicePath { get; set; }

        public override void ExecuteCmdlet()
        {
           var arpInfo = ExpressRouteClient.GetAzureDedicatedCircuitPeeringArpInfo(ServiceKey, AccessType, BgpPeeringDevicePath);
            Console.WriteLine(arpInfo);
            WriteObject(arpInfo);
        }
    }
}
