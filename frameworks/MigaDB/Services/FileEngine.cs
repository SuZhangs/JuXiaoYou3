namespace Acorisoft.FutureGL.MigaDB.Services
{
    public abstract class FileEngine : DataEngine, IFileEngine
    {
        protected FileEngine(string folderName)
        {
            
        }

        protected override void OnDatabaseClosing()
        {
        }

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
        }
        
        public string BaseDirectory { get; }
        public string FullDirectory { get; }
    }
}