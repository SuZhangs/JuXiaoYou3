using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class RepairToolViewModel : DialogViewModel
    {
        public RepairToolViewModel()
        {
            CreateShortcutCommand = Command(CreateShortcut);
            KillProcessCommand    = AsyncCommand(KillProcess);
            FixModuleCommand      = Command(FixModuleImpl);
        }

        public static Task KillProcess()
        {
            return Task.Run(ProcessUtilities.Kill);
        }

        public static void CreateShortcut()
        {
            var p = Process.GetCurrentProcess();
            if (p.MainModule is null)
            {
                return;
            }

            var dir         = AppDomain.CurrentDomain.BaseDirectory;
            var fileName    = p.MainModule.FileName;
            var lnkFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "橘小柚.lnk");
            FileIO.CreateShortcut(lnkFileName, fileName, dir);
        }

        public static void FixModuleImpl()
        {
            var te = Xaml.Get<IDatabaseManager>()
                         .GetEngine<TemplateEngine>();


            var inside = te.TemplateDB
                           .FindAll()
                           .ToArray();

            foreach (var module in inside)
            {
                
            }
            
            te.TemplateDB.DeleteAll();
            te.TemplateCacheDB.DeleteAll();
            te.MetadataCacheDB.DeleteAll();
            inside.ForEach(x => te.AddModule(x));
        }

        /// <summary>
        /// 修复模组
        /// </summary>
        public RelayCommand FixModuleCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand CreateShortcutCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand KillProcessCommand { get; }
    }
}