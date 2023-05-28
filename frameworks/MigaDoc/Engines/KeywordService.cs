using System.Collections;
using Acorisoft.Miga.Doc.Keywords;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class KeywordService : StorageService, IRefreshSupport
    {
        public KeywordService()
        {
            IdMapping   = new Dictionary<string, Keyword>();
            NameMapping = new Dictionary<string, Keyword>();
        }

        public void Refresh()
        {
            IdMapping.Clear();
        }


        public bool GetKeyword(string id, out Keyword keyword)
        {
            return IdMapping.TryGetValue(id, out keyword);
        }

        public IEnumerable<KeywordMapping> GetMapping(Keyword value)
        {
            return RelDB.Find(Query.EQ(nameof(KeywordMapping.Keyword), value.Id));
        }

        public bool Add(string newName, string owner)
        {
            if (newName is null)
            {
                return false;
            }

            if (NameMapping.ContainsKey(newName))
            {
                return false;
            }


            var keyword = new Keyword
            {
                Name  = newName,
                Id    = ShortGuidString.GetId(),
                Owner = owner
            };

            if (!KeywordDB.Contains(keyword.Id))
            {
                IdMapping.TryAdd(keyword.Id, keyword);
                NameMapping.TryAdd(keyword.Name, keyword);
            }

            KeywordDB.Upsert(keyword);
            return true;
        }
        
        private void Add(Keyword keyword)
        {
            if (keyword is null)
            {
                return;
            }
            KeywordDB.Upsert(keyword);
        }

        public bool Rename(Keyword keyword, string oldName, string newName)
        {
            if (NameMapping.ContainsKey(newName))
            {
                return false;
            }

            NameMapping.Remove(oldName);
            NameMapping.Add(newName, keyword);
            keyword.Name = newName;
            Add(keyword);
            return true;
        }

        public void Add(Sight sight)
        {
            if (sight is null)
            {
                return;
            }


            SightDB.Upsert(sight);
        }

        public bool Add(KeywordMapping mapping)
        {
            if (mapping is null)
            {
                return false;
            }

            // confuse :
            // 如果标签不存在就返回
            if (!IdMapping.ContainsKey(mapping.Keyword))
            {
                return false;
            }

            RelDB.Insert(mapping);
            return true;
        }

        public void Remove(Keyword keyword)
        {
            if (keyword is null)
            {
                return;
            }

            KeywordDB.Delete(keyword.Id);
            RelDB.DeleteMany(Query.EQ(nameof(KeywordMapping.Keyword), keyword.Id));
            IdMapping.Remove(keyword.Id);
            NameMapping.Remove(keyword.Name);
        }

        public void Remove(string labelName, string id)
        {
            if (!NameMapping.TryGetValue(labelName, out var keyword))
            {
                return;
            }

            var rel = RelDB.FindOne(Query.And(
                Query.EQ(nameof(KeywordMapping.Document), id),
                Query.EQ(nameof(KeywordMapping.Keyword), keyword.Id)));

            if (rel is null)
            {
                return;
            }

            //
            //
            RelDB.Delete(rel.Id);
        }

        public void Remove(Sight sight)
        {
            if (sight is null)
            {
                return;
            }

            SightDB.Delete(sight.Id);
        }

        public void Remove(DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }

            RelDB.DeleteMany(Query.EQ(nameof(KeywordMapping.Document), index.Id));

            Refresh();
        }

        public void Remove(string labelName, DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(labelName))
            {
                return;
            }

            if (!IdMapping.TryGetValue(labelName, out var keyword))
            {
                return;
            }

            RelDB.DeleteMany(
                Query.And(
                    Query.EQ(nameof(KeywordMapping.Keyword), keyword.Id),
                    Query.EQ(nameof(KeywordMapping.Document), index.Id)));

            Refresh();
        }

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            KeywordDB = context.Database.GetCollection<Keyword>(Constants.cn_keyword);
            RelDB     = context.Database.GetCollection<KeywordMapping>(Constants.cn_keywordMapping);
            SightDB   = context.Database.GetCollection<Sight>(Constants.cn_sight);

            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            KeywordDB = null;
            SightDB   = null;
            RelDB     = null;

            NameMapping.Clear();
            IdMapping.Clear();
        }

        public Dictionary<string, Keyword> IdMapping { get; }
        public Dictionary<string, Keyword> NameMapping { get; }
        public ILiteCollection<Keyword> KeywordDB { get; private set; }
        public ILiteCollection<Sight> SightDB { get; private set; }
        public ILiteCollection<KeywordMapping> RelDB { get; private set; }
    }
}