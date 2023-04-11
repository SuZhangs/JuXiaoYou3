using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class FileIO
    {
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
    }
}