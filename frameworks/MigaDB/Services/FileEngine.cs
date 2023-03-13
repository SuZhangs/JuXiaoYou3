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

        public bool HasFile(string md5)
        {
            return !string.IsNullOrEmpty(md5) &&
                   Records is not null &&
                   Records.HasID(md5);
        }
        
        public void AddFile(FileRecord record)
        {
            if (record is null)
            {
                return;
            }

            Records.Insert(record);
        }
        
        public void RemoveFile(FileRecord record)
        {
            if (record is null)
            {
                return;
            }

            Records.Delete(record.Id);
        }

        protected override void OnDatabaseClosing()
        {
            FullDirectory = null;
            Records       = null;
        }

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            Records       = session.Database.GetCollection<FileRecord>(Constants.Name_FileTable);
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

        public ILiteCollection<FileRecord> Records { get; private set; }

        public string BaseDirectory { get; }
        public string FullDirectory { get; private set; }
    }
}