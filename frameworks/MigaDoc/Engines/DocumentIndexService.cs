
namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class DocumentIndexService : StorageService, IRefreshSupport
    {
        public DocumentIndexService()
        {
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
        }
        
        public void Refresh()
        {
            Characters.Clear();
            Mapping.Clear();

            foreach (var index in Database.FindAll())
            {
                Mapping.Add(index.Id, index);
            }
        }


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
        }
        
        public List<DocumentIndex> Characters { get; }
        public Dictionary<string, DocumentIndex> Mapping { get; }
        public ILiteCollection<DocumentIndex> Database { get; private set; }
    }
}