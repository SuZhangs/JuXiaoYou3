using System.Data;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class NamePoolService : StorageService, IRefreshSupport, IDirectlyDifferenceProvider
    {
        public NamePoolService()
        {
            Collection = new SourceList<Moniker>();
            Mapping    = new Dictionary<string, Moniker>(13);
            IsLazyMode = true;
        }
        
        public void Refresh()
        {
           Collection.Clear();
           Mapping.Clear();
           foreach (var moniker in Database.FindAll())
           {
               if (Mapping.TryAdd(moniker.Name, moniker))
               {
                   Collection.Add(moniker);
               }
           }
        }       
        
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Moniker} dt) return;
            Database.Upsert(DeserializeFromBase64<Moniker>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Moniker} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.Moniker, this);
        }
        

        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.Moniker
            }));
        }

        public void Add(Moniker moniker)
        {
            if (moniker is null || string.IsNullOrEmpty(moniker.Name))
            {
                return;
            }

            if (Mapping.TryAdd(moniker.Name, moniker))
            {
                Collection.Add(moniker);
            }
            Database.Upsert(moniker);
            
        }
        
        public void Remove(Moniker moniker)
        {
            if (moniker is null || string.IsNullOrEmpty(moniker.Name))
            {
                return;
            }

            Mapping.Remove(moniker.Name);
            Collection.Remove(moniker);
            Database.Delete(moniker.Id);
            
        }
        
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<Moniker>(Constants.cn_moniker);
            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Collection.Clear();
            Mapping.Clear();
            Database = null;
        }
        
        public SourceList<Moniker> Collection { get; }
        public Dictionary<string, Moniker> Mapping { get; }
        public ILiteCollection<Moniker> Database { get; private set; }
    }
}