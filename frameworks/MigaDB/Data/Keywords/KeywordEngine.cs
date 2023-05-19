using System.Diagnostics.CodeAnalysis;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaDB.Data.Keywords
{
    [SuppressMessage("ReSharper", "CommentTypo")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class KeywordEngine : DataEngine
    {
        /*
         * Keyword
         * 1. ID == Name
         * 2. ReferenceCount == 0，删除
         * 3. ReferenceCount ++ , When add
         */
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
                // 如果有同名的目录，就添加映射
                var dir = DirectoryDB.Find(x => x.Name == keyword)
                                     .FirstOrDefault();

                if (dir is null)
                {
                    return;
                }
                
                var mapping = new DocumentMapping
                {
                    Id = ID.Get(),
                    DocumentID = documentID,
                    DirectoryID = dir.Id
                };
                MappingDB.Insert(mapping);
            }

            var k = KeywordDB.FindById(keyword);
            if (k is not null)
            {
                k.ReferenceCount++;
                KeywordDB.Update(k);
                return;
            }

            KeywordDB.Insert(new Keyword
            {
                Id   = keyword,
                Name = keyword,
                ReferenceCount = 1
            });
        }

        public void RemoveKeyword(string documentID, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return;
            }

            var dir = DirectoryDB.Find(x => x.Name == keyword)
                                 .FirstOrDefault();
            if (dir is null)
            {
                /*
                 * {
                 *      id = xxxxx,
                 *      documentID = documentID,
                 *      directoryID = directoryID 
                 * }
                 */
                MappingDB.DeleteMany(x => x.DirectoryID == dir.Id &&
                                          x.DocumentID == documentID);
            }

            //
            //
            var k = KeywordDB.FindById(keyword);
            k.ReferenceCount--;

            //
            //
            if (k.ReferenceCount == 0)
            {
                KeywordDB.Delete(keyword);
            }

        }
 
        public void AddDirectory(Keyword keyword)
        {
            if (keyword is null)
            {
                // 参数为空 或者 关键字为空
                return;
            }

            var name = keyword.Name;
            
            if (string.IsNullOrEmpty(name) ||
                DirectoryDB.HasName(name))
            {
                // 1. 名字为空
                // 2. DirectoryDB有同名的目录
                return;
            }

            var dir = new Directory
            {
                Id   = ID.Get(),
                Name = name
            };

            DirectoryDB.Insert(dir);
            KeywordDB.Delete(keyword.Id);
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

            var dir = new Directory
            {
                Id    = ID.Get(),
                Name  = name
            };

            DirectoryDB.Insert(dir);
        }

        public void AddDirectory(string name, Directory parent)
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

            if (DirectoryDB.HasID(name))
            {
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

        public IEnumerable<DocumentMapping> GetDocumentMappings(Directory directory)
        {
            if (directory is null ||
                !DirectoryDB.HasID(directory.Id))
            {
                return Array.Empty<DocumentMapping>();
            }

            return MappingDB.Find(x => x.DirectoryID == directory.Id);
        }

        public IEnumerable<Directory> GetDirectories() => DirectoryDB.FindAll();


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