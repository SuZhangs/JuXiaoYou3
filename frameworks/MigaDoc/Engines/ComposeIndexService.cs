using ComposeIndex = Acorisoft.Miga.Doc.Documents.ComposeIndex;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class ComposeIndexService : StorageService, IRefreshSupport, IDirectlyDifferenceProvider
    {
        public ComposeIndexService()
        {
            Collection = new SourceList<ComposeIndex>();
        }

        public void Add(ComposeIndex index)
        {
            if (index is null)
            {
                return;
            }

            if (!Database.Contains(index.Id))
            {
                Collection.Add(index);
            }

            Database.Upsert(index);
        }
        
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.ComposeIndex, this);
        }
        

        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.ComposeIndex
            }));
        }
        
        
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.ComposeIndex} dt) return;
            Database.Upsert(DeserializeFromBase64<ComposeIndex>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.ComposeIndex} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }

        public void Remove(ComposeIndex index)
        {          
            if (index is null)
            {
                return;
            }
            
            Collection.Remove(index);
            Database.Delete(index.Id);
        }

        public void Refresh()
        {
            Collection.Clear();
            Collection.AddRange(Database.FindAll());
        }

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<ComposeIndex>(Constants.cn_index_compose);
            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Database = null;
            Collection.Clear();
        }

        public SourceList<ComposeIndex> Collection { get; }
        public ILiteCollection<ComposeIndex> Database { get; private set; }
    }
}