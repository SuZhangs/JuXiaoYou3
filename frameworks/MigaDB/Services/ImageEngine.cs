using Acorisoft.FutureGL.MigaDB.IO;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public class ImageEngine : FileEngine
    {
        public ImageEngine() : base(Constants.ImageFolderName)
        {
            
        }

        public void SetAvatar(MemoryStream ms, string resource)
        {
            if (ms is null || ms.Length == 0)
            {
                return;
            }

            if (resource is null)
            {
                return;
            }

            var dst = Path.Combine(FullDirectory, resource);
            var fs  = new FileStream(dst, FileMode.Create, FileAccess.Write);
            
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);
            fs.Dispose();
        }
    }
}