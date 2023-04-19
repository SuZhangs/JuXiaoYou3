using System.Threading.Tasks;
using System.Windows;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class FileIO
    {
        public static async Task CaptureAsync(FrameworkElement element)
        {
            if (element is null)
            {
                return;
            }

            var savedlg = Save(SubSystemString.PngFilter, "*.png");

            if (savedlg.ShowDialog() != true)
            {
                return;
            }
            
            var ms = Xaml.CaptureToStream(element);
            await System.IO.File.WriteAllBytesAsync(savedlg.FileName, ms.GetBuffer());
        }
        
        public static VistaSaveFileDialog Save(string filter, string defaultExt, string fileName = null)
        {
            return new VistaSaveFileDialog
            {
                FileName     = fileName,
                Filter       = filter,
                AddExtension = true,
                DefaultExt   = defaultExt
            };
        }

        public static VistaOpenFileDialog Open(string filter, bool multiselect = false)
        {
           return new VistaOpenFileDialog
            {
                Filter      = filter,
                Multiselect = multiselect
            };
        }
        /// <summary>
        /// 创建一个快捷方式
        /// </summary>
        /// <param name="lnkFilePath">快捷方式的完全限定路径。</param>
        /// <param name="workDir"></param>
        /// <param name="args">快捷方式启动程序时需要使用的参数。</param>
        /// <param name="targetPath"></param>
        public static void CreateShortcut(string lnkFilePath, string targetPath, string workDir, string args = "")
        {
            var     shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell     = Activator.CreateInstance(shellType);
            var     shortcut  = shell.CreateShortcut(lnkFilePath);
            shortcut.TargetPath       = targetPath;
            shortcut.Arguments        = args;
            shortcut.WorkingDirectory = workDir;
            shortcut.Save();
        }
        
        /// <summary>
        /// 读取一个快捷方式的信息
        /// </summary>
        /// <param name="lnkFilePath"></param>
        /// <returns></returns>
        public static ShortcutDescription ReadShortcut(string lnkFilePath)
        {
            var     shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell     = Activator.CreateInstance(shellType);
            var     shortcut  = shell.CreateShortcut(lnkFilePath);
            return new ShortcutDescription()
            {
                TargetPath = shortcut.TargetPath,
                Arguments  = shortcut.Arguments,
                WorkingDirection    = shortcut.WorkingDirectory,
            };
        }
    }

    public class ShortcutDescription
    {
        public object TargetPath { get; set; }
        public object Arguments { get; set; }
        public object WorkingDirection { get; set; }
    }
}