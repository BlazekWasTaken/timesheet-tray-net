using System;
using System.IO;
using WixSharp;
using WixSharp.CommonTasks;
using File = WixSharp.File;

namespace timesheet_tray_net.Wix
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Build();
        }
        
        private static void Build()
        {
            const string authorName = "BlazekWasTaken";
            const string appName = "timesheet-tray-net";
            const string appVersion = "0.1.0";
            var appGuid = new Guid("A49BF951-75E4-4CB2-8A50-87B8293B2EC9");
            var exe = $@"C:\Users\blazej\Desktop\{appName}.exe";
            var myAppId = new Id("timesheet_tray_net_exe");
            
            WixTools.AcceptEulaFor = "wix7";
            WixTools.SetWixVersion(Environment.CurrentDirectory, "7.0.0");
            
            var project = new Project(appName,
                new Dir($@"%ProgramFiles%\{authorName}\{appName}",
                    new File(myAppId, exe)
                    ),
                new Dir("%ProgramMenu%", 
                    new ExeFileShortcut(appName, Path.Combine("[INSTALLDIR]", $"{appName}.exe"), arguments: "") 
                        { WorkingDirectory = "[INSTALLDIR]" },
                    new Dir("Startup",
                        new ExeFileShortcut(appName, Path.Combine("[INSTALLDIR]", $"{appName}.exe"), arguments: "") 
                            { WorkingDirectory = "[INSTALLDIR]" })))
            {
                Scope = InstallScope.perMachine,
                MajorUpgrade = new MajorUpgrade
                {
                    AllowSameVersionUpgrades = true,
                    DowngradeErrorMessage = $"A newer version of {appName} is already installed."
                },
                Version = new Version(appVersion),
                GUID = appGuid,
                Platform = Platform.x64,
                ControlPanelInfo = new ProductInfo
                {
                    Manufacturer = authorName,
                    Comments = "Tray application for tracking working hours"
                },
                UI = WUI.WixUI_ProgressOnly
            };

            Compiler.BuildMsi(project);
        }
    }
}