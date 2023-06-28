using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Others;

namespace Acorisoft.FutureGL.MigaDB.Data
{
    public class EntityEngine : DataEngine
    {
        public void AddBookmark(Bookmark bookmark)
        {
            if (bookmark is null)
            {
                return;
            }

            BookmarkDB.Insert(bookmark);
        }

        public IEnumerable<Bookmark> GetBookmarks() => BookmarkDB.FindAll();

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            var db = session.Database;
            BookmarkDB = db.GetCollection<Bookmark>(Constants.BookmarkName);
        }

        protected override void OnDatabaseClosing()
        {
            BookmarkDB = null;
        }
        
        public ILiteCollection<Bookmark> BookmarkDB { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ILiteCollection<DirectorySupport> BookmarkDirDB { get; private set; }
    }
}