using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Utils;
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

        #region Documents

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="document">指定要添加的文档</param>
        /// <returns>返回操作结果</returns>
        public EngineResult AddDocumentCache(DocumentCache document)
        {
            if (document is null)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            if (string.IsNullOrEmpty(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.MissingId);
            }

            if (DocumentDB.HasID(document.Id) ||
                DocumentCacheDB.HasID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.Duplicated);
            }


            DocumentCacheDB.Insert(document);

            //
            // 一致性检查
            //if (!DocumentDB.HasID(document.Id) ||
            //    !DocumentCacheDB.HasID(document.Id))
            //{
            //    return EngineResult.Failed(EngineFailedReason.ConsistencyBroken);
            //}

            //
            //
            Modified();
            return EngineResult.Successful;
        }

        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="document">指定要添加的文档</param>
        /// <returns>返回操作结果</returns>
        public EngineResult AddDocument(Document document)
        {
            if (document is null)
            {
                return EngineResult.Failed(EngineFailedReason.ParameterEmptyOrNull);
            }

            if (string.IsNullOrEmpty(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.MissingId);
            }

            if (document.Parts is null || document.Metas is null)
            {
                return EngineResult.Failed(EngineFailedReason.InputDataError);
            }

            if (DocumentDB.HasID(document.Id) ||
                DocumentCacheDB.HasID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.Duplicated);
            }
            
            DocumentDB.Insert(document);

            //
            // 一致性检查
            if (!DocumentDB.HasID(document.Id) ||
                !DocumentCacheDB.HasID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.ConsistencyBroken);
            }

            //
            //
            Modified();
            return EngineResult.Successful;
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        /// <param name="document">指定要移除的文档</param>
        public void RemoveDocument(Document document)
        {
            if (document is null)
            {
                return;
            }

            var cache = DocumentCacheDB.FindById(document.Id);


            if (cache is null)
            {
                return;
            }

            cache.IsDeleted      = true;
            cache.TimeOfModified = DateTime.Now;


            Modified();
            DocumentCacheDB.Update(cache);
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="document">指定要更新的文档</param>
        /// <param name="cache">指定要更新的文档</param>
        public void UpdateDocument(Document document, DocumentCache cache)
        {
            if (document is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(document.Id))
            {
                return;
            }

            if (document.Parts is null || document.Metas is null)
            {
                return;
            }

            if (!DocumentDB.HasID(document.Id) ||
                !DocumentCacheDB.HasID(document.Id))
            {
                return;
            }

            Modified();
            DocumentCacheDB.Update(cache);
            DocumentDB.Update(document);
        }

        
        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="cache">指定要更新的文档</param>
        public void UpdateDocument(DocumentCache cache)
        {
            if (cache is null)
            {
                return;
            }
        
            if (string.IsNullOrEmpty(cache.Id))
            {
                return;
            }
        
            if (!DocumentCacheDB.HasID(cache.Id))
            {
                return;
            }
        
            Modified();
            DocumentCacheDB.Update(cache);
        }

        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="id">指定的文档id</param>
        public Document GetDocument(string id)
        {
            return DocumentDB.FindById(id);
        }

        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="cache">指定的文档缓存</param>
        public Document GetDocument(DocumentCache cache)
        {
            if (cache is null)
            {
                return null;
            }

            return cache.IsDeleted ? null : GetDocument(cache.Id);
        }

        #endregion

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
        /// 清空所有文档
        /// </summary>
        public void ReduceSize()
        {
            var expression = Query.EQ(nameof(DocumentCache.IsDeleted), true);
            var markAsDeletedDocumentCache = DocumentCacheDB.Find(expression)
                                                            .Select(x => x.Id)
                                                            .ToHashSet();
            DocumentCacheDB.DeleteMany(x => x.IsDeleted);
            DocumentDB.DeleteMany(x => markAsDeletedDocumentCache.Contains(x.Id));


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