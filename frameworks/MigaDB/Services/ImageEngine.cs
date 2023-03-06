using Acorisoft.FutureGL.MigaDB.IO;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public class ImageEngine : FileEngine
    {
        public ImageEngine() : base(Constants.ImageFolderName)
        {
            
        }

        public void Write(byte[] buffer, Resource resource)
        {
            if (buffer is null)
            {
                return;
            }

            if (resource is null)
            {
                return;
            }

            var dst = Path.Combine(FullDirectory, resource.RelativePath);
            var fs  = new FileStream(dst, FileMode.Create, FileAccess.Write);
            
            fs.Write(buffer, 0, buffer.Length);
            fs.Dispose();
        }
    }
}