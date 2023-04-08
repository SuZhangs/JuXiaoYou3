namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    public class KeywordEngine : DataEngine
    {
        public void AddKeyword(string keyword)
        {
            // KeywordDB.Upsert(new Keyword
            // {
            //     Id = keyword,
            //     Value = keyword
            // });
        }
        
        public void RemoveKeyword(string keyword)
        {
            // KeywordDB.Delete(keyword);
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