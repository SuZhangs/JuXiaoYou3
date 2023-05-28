using ImTools;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class DocumentIndexService : StorageService, IRefreshSupport, IDirectlyDifferenceProvider
    {
        public DocumentIndexService()
        {
            Collection = new SourceList<DocumentIndex>();
            Mapping    = new Dictionary<string, DocumentIndex>(13);
            Characters = new List<DocumentIndex>(256);
        }

        public void Add(DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }

            if (Database.Contains(index.Id))
            {
                Database.Update(index);
            }
            else
            {
                if(index.DocumentType == DocumentKind.Character && (index.EntityType == EntityType.Document || index.EntityType == EntityType.Author))
                    Characters.Add(index);
                Collection.Add(index);
                Database.Insert(index);
                Mapping.Add(index.Id, index);
            }
        }

        public DocumentIndex Open(string id)
        {
            return string.IsNullOrEmpty(id) ? null : Database.FindById(id);
        }
        
        public void Remove(DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }
            
            
            if(index.DocumentType == DocumentKind.Character && index.EntityType == EntityType.Document)
                Characters.Remove(index);
            
            Database.Delete(index.Id);
            Collection.Remove(index);
        }
        
        public void Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            if(!Mapping.TryGetValue(id, out var index))
                return;
            
            if(index.DocumentType == DocumentKind.Character && 
               (index.EntityType == EntityType.Document || index.EntityType == EntityType.Author))
                Characters.Remove(index);
            Database.Delete(id);
            Collection.Remove(index);
        }
        
        public void Refresh()
        {
            Characters.Clear();
            Collection.Clear();
            Mapping.Clear();

            foreach (var index in Database.FindAll())
            {
                Mapping.Add(index.Id, index);
                Collection.Add(index);
                if(index.DocumentType == DocumentKind.Character && 
                   (index.EntityType == EntityType.Document || index.EntityType == EntityType.Author))
                    Characters.Add(index);
            }
        }

        public DocumentIndex GetAuthor(IRepositoryEngine engine)
        {
            var property = engine.Property;
            var authorId = property.IndexId;
            return Mapping.TryGetValue(authorId, out var author) ? author : null;
        }

        #region IDirectlyDifferenceProvider

        

        
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.DocumentIndex} dt) return;
            Database.Upsert(DeserializeFromBase64<DocumentIndex>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.DocumentIndex} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }
        
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.DocumentIndex, this);
        }

        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.DocumentIndex
            }));
        }
        
        #endregion
        
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<DocumentIndex>(Constants.cn_index);
            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Database = null;
            Mapping.Clear();
            Characters.Clear();
            Collection.Clear();
        }
        
        public List<DocumentIndex> Characters { get; }
        public Dictionary<string, DocumentIndex> Mapping { get; }
        public SourceList<DocumentIndex> Collection { get; }
        public ILiteCollection<DocumentIndex> Database { get; private set; }
    }
}