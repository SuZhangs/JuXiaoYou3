using Acorisoft.Miga.Doc.IO;
using Acorisoft.Miga.Doc.Networks;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class ImageService : DirectoryService, IBinaryDifferenceProvider
    {
        private const string SubFolders = "Images";

        public ImageService() : base(SubFolders)
        {
        }

        public ImageService(string dir) : base(SubFolders)
        {
            OnRepositoryOpening(new RepositoryContext
            {
                RepositoryFolder = dir
            }, null);
        }

        protected internal sealed override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            base.OnRepositoryOpening(context, property);
        }

        public string GetImageFileName(Resource resource)
        {
            if (string.IsNullOrEmpty(Directory)) return string.Empty;
            return resource is null ? string.Empty : Path.Combine(Directory, resource.Location);
        }

        public Task<Resource> DirectCopyAsync(string fileName)
        {
            return Task.Run(() => DirectCopy(fileName));
        }

        public Resource DirectCopy(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            if (!File.Exists(fileName))
            {
                return null;
            }

            var info = new FileInfo(fileName);
            var dst = Path.Combine(Directory, info.Name);
            var res = new Resource
            {
                Kind     = ResourceKind.Database,
                Location = info.Name,
            };

            if (File.Exists(dst) && new FileInfo(dst).Length == info.Length)
            {
                dst = Path.Combine(Directory, ShortGuidString.GetId() + info.Extension);
                res = new Resource
                {
                    Kind     = ResourceKind.Database,
                    Location = info.Name,
                };
            }
                
            File.Copy(fileName, dst, true);

            return res;
        }

        public Stream DirectOpen(Resource resource)
        {
            if (resource.Kind != ResourceKind.Database)
            {
                return null;
            }

            var dst = GetImageFileName(resource);

            if (!File.Exists(dst))
            {
                return null;
            }

            try
            {
                using var fs = new FileStream(dst, FileMode.Open);
                var capacity = (int)Math.Pow(2, Math.Log2(fs.Length));
                var ms = new MemoryStream(capacity);

                fs.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
            catch
            {
                return null;
            }
        }

        public void Resolve(Transaction transaction)
        {
            if (transaction is not BinaryTransaction bt) return;
            var buffer = Convert.FromBase64String(bt.Base64);
            File.WriteAllBytes(Path.Combine(Directory,bt.FileName), buffer);
        }

        public void Process(Transaction transaction)
        {
            if (transaction is not BinaryTransaction bt) return;
            var buffer = File.ReadAllBytes(Path.Combine(Directory,bt.FileName));
            bt.Base64 = Convert.ToBase64String(buffer);
        }

        public List<BinaryDescription> GetDescriptions()
        {
            return System.IO.Directory.GetFiles(Directory).Select(x =>
            {
                var fi = new FileInfo(x);
                return new BinaryDescription { FileName = fi.Name };
            }).ToList();
        }
    }
}