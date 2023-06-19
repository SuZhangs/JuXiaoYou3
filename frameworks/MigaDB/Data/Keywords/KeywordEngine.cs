﻿using System.Diagnostics.CodeAnalysis;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class KeywordEngine : DataEngine
    {
        public void AddKeyword(Keyword keyword)
        {
            if (keyword is null ||
                string.IsNullOrEmpty(keyword.Name) ||
                string.IsNullOrEmpty(keyword.DocumentId))
            {
                return;
            }

            KeywordDB.Upsert(keyword);
        }

        public void RemoveKeyword(string documentID, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return;
            }

            var v = KeywordDB.Find(x => x.DocumentId == documentID && x.Name == keyword)
                             .FirstOrDefault();

            if (v is null)
            {
                return;
            }

            KeywordDB.Delete(v.Id);
        }
        
        public void RemoveKeyword(Keyword keyword)
        {
            if (keyword is null ||
                string.IsNullOrEmpty(keyword.Id))
            {
                return;
            }

            KeywordDB.Delete(keyword.Id);
        }

        /// <summary>
        /// 删除文档的时候，调用该方法删除所有的关键字引用
        /// </summary>
        /// <param name="documentId"></param>
        public void RemoveMappings(string documentId)
        {
            KeywordDB.DeleteMany(x => x.DocumentId == documentId);
        }
        
        /// <summary>
        /// 是否有关键字
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasKeyword(string documentId, string name)
        {
            return KeywordDB.Exists(x => x.DocumentId == documentId && x.Name == name);
        }

        /// <summary>
        /// 获得某个文档的所有关键字
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public IEnumerable<Keyword> GetKeywords(string documentId) => KeywordDB.Find(x => x.DocumentId == documentId);

        public int GetKeywordCount(string documentId) => KeywordDB.Count(x => x.DocumentId == documentId);

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            DirectoryDB = session.Database.GetCollection<Directory>(Constants.Name_Directory);
            KeywordDB   = session.Database.GetCollection<Keyword>(Constants.Name_Keyword);
        }

        protected override void OnDatabaseClosing()
        {
            DirectoryDB = null;
            KeywordDB   = null;
        }

        /// <summary>
        /// 模板
        /// </summary>
        public ILiteCollection<Directory> DirectoryDB { get; private set; }

        /// <summary>
        /// 模板缓存
        /// </summary>
        public ILiteCollection<Keyword> KeywordDB { get; private set; }
    }
}