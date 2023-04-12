using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class RepairToolViewModel : DialogViewModel
    {
        public RepairToolViewModel()
        {
            CreateShortcutCommand = Command(CreateShortcut);
            KillProcessCommand    = AsyncCommand(KillProcess);
        }
        
        public static Task KillProcess()
        {
            return Task.Run(ProcessUtilities.Kill);
        }

        public static void CreateShortcut()
        {
            var p        = Process.GetCurrentProcess();
            if (p.MainModule is  null)
            {
                return;
            }

            var dir         = AppDomain.CurrentDomain.BaseDirectory;
            var fileName    = p.MainModule.FileName;
            var lnkFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "橘小柚.lnk");
            FileIO.CreateShortcut(lnkFileName, fileName, dir);
        }

        public RelayCommand CreateShortcutCommand { get; } 
        public AsyncRelayCommand KillProcessCommand { get; }
    }
}