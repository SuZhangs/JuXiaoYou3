﻿using Acorisoft.FutureGL.MigaDB.IO;

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

            var dst = Path.Combine(FullDirectory, $"{resource}.png");
            var fs  = new FileStream(dst, FileMode.Create, FileAccess.Write);
            
            ms.CopyTo(fs);
            fs.Dispose();
        }
    }
}