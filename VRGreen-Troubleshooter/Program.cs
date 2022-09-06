using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace VRGreen_Troubleshooter
{
    internal class Program
    {
        // Check Real Time Protection
        // Check Core Isolation
        // Check Medal 
        // Check Windows Build
        // Check Anti Cheats (FACEIT)
        // Check Anti Viruses
        static void Main(string[] args)
        {
            Console.Title = "VRGreen Troubleshooter by Sypherr#0001";

            Helpers.CheckRealTimeProtectionState();
            Helpers.CheckCoreIsolationState();
            Helpers.CheckForWindowsBuild();
            Helpers.CheckCompatibility();
            Helpers.CheckForAntiVirus();
            Console.ReadKey();
        }
    }
}
