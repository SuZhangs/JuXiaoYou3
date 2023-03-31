using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Services
{
    public abstract class FileEngine : DataEngine, IFileEngine
    {
        protected FileEngine(string folderName)
        {
            BaseDirectory = folderName;
        }

        public string GetFileName(string id) => Path.Combine(FullDirectory, id);

        public MemoryStream Get(string id)
        {
            id = GetFileName(id);
            if (File.Exists(id))
            {
                var buffer = File.ReadAllBytes(id);
                return new MemoryStream(buffer);
            }

            return null;
        }

        protected override void OnDatabaseClosing()
        {
            FullDirectory = null;
        }

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            FullDirectory = Path.Combine(session.RootDirectory, BaseDirectory);
            
            if (!Directory.Exists(session.RootDirectory))
            {
                Directory.CreateDirectory(session.RootDirectory);
            }
            
            if (!Directory.Exists(FullDirectory))
            {
                Directory.CreateDirectory(FullDirectory);
            }
        }

        public string BaseDirectory { get; }
        public string FullDirectory { get; private set; }
    }
}