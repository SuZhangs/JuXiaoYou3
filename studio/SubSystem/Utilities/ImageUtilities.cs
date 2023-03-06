using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.IO;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public class ImageOpResult
    {
        public bool IsFinished { get; init; }
        public Resource Resource { get; init; }
        public string FileName { get; init; }
        public byte[] Buffer { get; init; }
    }
    
    public class ImageUtilities
    {
        public static async Task<ImageOpResult> Avatar()
        {
            var opendlg = new VistaOpenFileDialog
            {
                Filter      = StringFromCode.ImageFilter,
                Multiselect = false
            };

            if (opendlg.ShowDialog() != true)
            {
                return new ImageOpResult { IsFinished = false };
            }
        }
    }
}