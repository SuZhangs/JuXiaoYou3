using static Acorisoft.FutureGL.MigaDB.Constants;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    public class DocumentEngine : DataEngine
    {
        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            var database = session.Database;
            DocumentDB      = database.GetCollection<Document>(Name_Document);
            DocumentCacheDB = database.GetCollection<DocumentCache>(Name_Cache_Document);
            ComposeDB       = database.GetCollection<Compose>(Name_Compose);
            ComposeCacheDB  = database.GetCollection<ComposeCache>(Name_Cache_Compose);
        }

        protected override void OnDatabaseClosing()
        {
            DocumentDB      = null;
            DocumentCacheDB = null;
            ComposeDB       = null;
            ComposeCacheDB  = null;
        }

        public void AddDocument(Document document)
        {
            
        }

        public void RemoveDocument(Document document)
        {
            
        }

        public void UpdateDocument(Document document)
        {
            
        }

        public void AddCompose(Compose document)
        {
            
        }

        public void RemoveCompose(Compose document)
        {
            
        }

        public void UpdateCompose(Compose document)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<DocumentCache> DocumentCacheDB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<Document> DocumentDB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<ComposeCache> ComposeCacheDB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<Compose> ComposeDB { get; private set; }
    }
}