using System.Diagnostics.CodeAnalysis;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class KeywordEngine : DataEngine
    {
        public void AddKeyword(string documentID, string keyword)
        {
            if (string.IsNullOrEmpty(keyword) ||
                string.IsNullOrEmpty(documentID))
            {
                return;
            }

            KeywordDB.Upsert(new Keyword
            {
                Id         = ID.Get(),
                DocumentId = documentID,
                Name       = keyword,
            });
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

        public void RemoveMappings(string documentId)
        {
            KeywordDB.DeleteMany(x => x.DocumentId == documentId);
        }


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