using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Concepts;
using static Acorisoft.FutureGL.MigaDB.Constants;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    [ConceptProvider]
    public class ComposeEngine : KnowledgeEngine
    {

        public void AddCompose(Compose document)
        {
        }

        public void RemoveCompose(Compose document)
        {
        }

        public void UpdateCompose(Compose document)
        {
        }

        public override Knowledge GetKnowledge(string id)
        {
            throw new NotImplementedException();
        }

        protected override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
            var database = session.Database;
            ComposeDB       = database.GetCollection<Compose>(Name_Compose);
            ComposeCacheDB  = database.GetCollection<ComposeCache>(Name_Cache_Compose);
        }

        protected override void OnDatabaseClosingOverride()
        {
            ComposeDB       = null;
            ComposeCacheDB  = null;
        }


        /// <summary>
        /// 清空所有文档
        /// </summary>
        public void ReduceSize()
        {
            var expression = Query.EQ(nameof(DocumentCache.IsDeleted), true);

            var markAsDeletedComposeCache = ComposeCacheDB.Find(expression)
                                                          .Select(x => x.Id)
                                                          .ToHashSet();
            ComposeCacheDB.DeleteMany(x => x.IsDeleted);
            ComposeDB.DeleteMany(x => markAsDeletedComposeCache.Contains(x.Id));

            Modified();
        }

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