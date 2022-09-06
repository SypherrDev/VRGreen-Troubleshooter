using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Management;

namespace VRGreen_Troubleshooter
{
    internal class Helpers
    {
        public static void CheckRealTimeProtectionState()
        {
            try
            {
                RegistryKey reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                if (reg != null)
                {
                    var value = reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows Defender\Real-Time Protection").GetValue("DisableRealtimeMonitoring");
                    if (value != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[RealTimeProtection]: Your Windows Defender real-time protection is off.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[RealTimeProtection]: Your Windows Defender real-time protection is ON!");
                        Console.ResetColor();
                    }
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[RealTimeProtection]: Something went wrong checking for RealTimeProtectionState!");
                Console.ResetColor();
            }
        }

        public static void CheckForAntiVirus()
        {
            ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectCollection data = wmiData.Get();
            foreach (ManagementObject virusChecker in data)
            {
                Console.WriteLine($"[AntiVirusDetection]: {virusChecker["displayName"]}");
            }
        }

        public static void CheckCoreIsolationState()
        {
            try
            {
                RegistryKey reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                if (reg != null)
                {
                    var value = reg.OpenSubKey(@"Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity");
                    if (value == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[CoreIsolation]: Your Core Isolation is off.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[CoreIsolation]: Your Core Isolation is ON!");
                        Console.ResetColor();
                    }
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[CoreIsolation]: Something went wrong checking for CheckCoreIsolationState!");
                Console.ResetColor();
            }
        }

        public static void CheckForWindowsBuild()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            var _CurrentBuild = registryKey.GetValue("CurrentBuild").ToString();
            if (int.Parse(_CurrentBuild) < 19000 || int.Parse(_CurrentBuild) > 22000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[WindowsBuild]: Your window version is NOT compatible with VRGreen. [{_CurrentBuild}]");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[WindowsBuild]: Your window version is compatible with VRGreen. [{_CurrentBuild}]");
                Console.ResetColor();
            }
        }

        public static void CheckCompatibility()
        {
            string[] apps = new string[] { "medal", "faceit" }; // I do this incase we find another program that is not compatible.

            foreach (string appName in apps)
            {
                Process[] p = Process.GetProcessesByName(appName);
                if (p.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[CompatibilityChecker]: {appName} is not running!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[CompatibilityChecker]: {appName} is running!");
                    Console.ResetColor();
                }
            }

            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        foreach (var app in apps)
                        {
                            try
                            {
                                if (subkey.GetValue("DisplayName").ToString().Contains(app))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"[CompatibilityChecker]: {app} is detected!");
                                    Console.ResetColor();
                                }
                            }
                            catch{ }
                        }
                    }
                }
            }
        }
    }
}