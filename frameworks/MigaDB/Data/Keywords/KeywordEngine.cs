using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    public class KeywordEngine : DataEngine
    {
        public void AddKeyword(string documentID, string keyword)
        {
            if (string.IsNullOrEmpty(keyword) ||
                string.IsNullOrEmpty(documentID))
            {
                return;
            }

            if (DirectoryDB.HasName(keyword))
            {
                //
                // 映射内容
                return;
            }

            if (KeywordDB.HasName(keyword))
            {
                return;
            }

            // KeywordDB.Upsert(new Keyword
            // {
            //     Id = keyword,
            //     Value = keyword
            // });
        }

        public void RemoveKeyword(string documentID, string keyword)
        {
            // KeywordDB.Delete(keyword);
        }

        public void AddDirectory(string name)
        {
            if (string.IsNullOrEmpty(name) ||
                !DirectoryDB.HasName(name))
            {
                //
                // 映射内容
                return;
            }
        }

        public EngineResult AddDirectory(string name, Directory parent)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            
            if (parent is null ||
                string.IsNullOrEmpty(parent.Id) ||
                !DirectoryDB.HasID(parent.Id))
            {
                //
                // 映射内容
                return;
            }

            var dir = new Directory
            {
                Id    = ID.Get(),
                Owner = parent.Id,
                Name  = name
            };

            DirectoryDB.Insert(dir);
        }
        
        public void ChangeDirectory(Directory directory)
        {
            if (directory is null ||
                string.IsNullOrEmpty(directory.Id))
            {
                return;
            }
            
            if (!DirectoryDB.HasName(directory.Name))
            {
                //
                // 映射内容
                return;
            }
            
            DirectoryDB.Update(directory);
        }
        
        public void ChangeDirectory(Directory directory, Directory parent)
        {
            if (directory is null ||
                string.IsNullOrEmpty(directory.Id))
            {
                return;
            }
            
            if (parent is null ||
                string.IsNullOrEmpty(parent.Id))
            {
                return;
            }
            
            if (!DirectoryDB.HasName(directory.Name))
            {
                //
                // 映射内容
                return;
            }

            directory.Owner = parent.Id;
            DirectoryDB.Update(directory);
        }

        public void RemoveDirectory(Directory directory, bool addToParent)
        {
            if (directory is null ||
                string.IsNullOrEmpty(directory.Id))
            {
                return;
            }

            DirectoryDB.Delete(directory.Id);
            RemoveMappings(directory.Id, addToParent);
        }

        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            MappingDB   = session.Database.GetCollection<DocumentMapping>(Constants.Name_Relationship_Directory);
            DirectoryDB = session.Database.GetCollection<Directory>(Constants.Name_Directory);
            KeywordDB   = session.Database.GetCollection<Keyword>(Constants.Name_Keyword);
        }

        protected override void OnDatabaseClosing()
        {
            DirectoryDB = null;
            KeywordDB   = null;
        }

        public void RemoveMappings(string id, bool addedToParent)
        {
            // 用于表示当前的操作是删除节点操作
            var targetIsDirectory = DirectoryDB.HasID(id);

            if (targetIsDirectory)
            {
                var directory = DirectoryDB.FindById(id);
                var parent    = directory.Owner;
                var children  = DirectoryDB.Find(x => x.Owner == id);

                if (!string.IsNullOrEmpty(directory.Owner) && addedToParent)
                {
                    //
                    // 添加到父级子节点
                    children.ForEach(x => x.Owner = parent);
                }
                else
                {
                    var hash = children
                               .Select(x => x.Id)
                               .ToHashSet();
                    //
                    // 删除所有子节点
                    DirectoryDB.DeleteMany(x => hash.Contains(x.Id));
                }

                //
                // 删除所有映射
                MappingDB.DeleteMany(x => x.DirectoryID == id);
            }
            else
            {
                //
                // 删除所有映射
                MappingDB.DeleteMany(x => x.DocumentID == id);
            }
        }


        /// <summary>
        /// 映射
        /// </summary>
        public ILiteCollection<DocumentMapping> MappingDB { get; private set; }

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