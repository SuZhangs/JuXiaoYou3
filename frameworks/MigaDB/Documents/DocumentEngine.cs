using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Concepts;
using Acorisoft.FutureGL.MigaDB.Utils;
using static Acorisoft.FutureGL.MigaDB.Constants;

namespace Acorisoft.FutureGL.MigaDB.Documents
{
    [ConceptProvider]
    public class DocumentEngine : KnowledgeEngine
    {
        protected override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
            var database = session.Database;
            DocumentDB      = database.GetCollection<Document>(Name_Document);
            DocumentCacheDB = database.GetCollection<DocumentCache>(Name_Cache_Document);
        }

        protected override void OnDatabaseClosingOverride()
        {
            DocumentDB      = null;
            DocumentCacheDB = null;
        }

        

        public override Knowledge GetKnowledge(string id)
        {
            var cache = DocumentCacheDB.FindById(id);

            if (cache is null)
            {
                return null;
            }

            return new Knowledge
            {
                Id     = cache.Id,
                Name   = cache.Name,
                Intro  = cache.Intro,
                Avatar = cache.Avatar
            };
        }

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


            AddConcept(document.Id, document.Name, DataEngineType.DocumentEngine);
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

            if (!DocumentCacheDB.HasID(document.Id))
            {
                return EngineResult.Failed(EngineFailedReason.ConsistencyBroken);
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
            RemoveConcept(document.Id);
            DocumentCacheDB.Update(cache);
        }
        
        
        /// <summary>
        /// 移除文档
        /// </summary>
        /// <param name="cache">指定要移除的文档</param>
        public void RemoveDocumentCache(DocumentCache cache)
        {
            if (cache is null)
            {
                return;
            }

            cache.IsDeleted      = true;
            cache.TimeOfModified = DateTime.Now;
            DocumentCacheDB.Update(cache);
            RemoveConcept(cache.Id);
            Modified();
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
                !DocumentCacheDB.HasID(cache.Id))
            {
                return;
            }

            AddConcept(document.Id, document.Name, DataEngineType.DocumentEngine);
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
        
            AddConcept(cache.Id, cache.Name, DataEngineType.DocumentEngine);
            DocumentCacheDB.Update(cache);
        }
        
        /// <summary>
        /// 获得指定的文档
        /// </summary>
        public IEnumerable<DocumentCache> GetDocuments()
        {
            return DocumentCacheDB.Find(x => !x.IsDeleted);
        }
        
        /// <summary>
        /// 获得指定的文档
        /// </summary>
        /// <param name="type">指定的文档id</param>
        public IEnumerable<DocumentCache> GetDocuments(DocumentType type)
        {
            return DocumentCacheDB.Find(x => !x.IsDeleted && x.Type == type);
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

            if(markAsDeletedDocumentCache.Count > 0)
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
    }
}