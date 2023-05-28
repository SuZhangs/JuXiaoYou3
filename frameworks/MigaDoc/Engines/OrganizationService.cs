using Acorisoft.Miga.Doc.Entities.Organizations;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class OrganizationService : StorageService, IRefreshSupport, IDirectlyDifferenceProvider
    {
        public OrganizationService()
        {
            IsLazyMode = true;
            Collection = new SourceList<Organization>();
            Mapping    = new Dictionary<string, Organization>(7);
        }
        
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.Organization, this);
        }
        
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Organization} dt) return;
            Database.Upsert(DeserializeFromBase64<Organization>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Organization} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }
        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.Organization
            }));
        }

        public void Refresh()
        {
            Collection.Clear();
            foreach (var org in Database.FindAll())
            {
                Collection.Add(org);
                Mapping.TryAdd(org.Name, org);
            }
        }

        public void Add(Organization org)
        {
            if (org is null)
            {
                return;
            }

            if (!Database.Contains(org.Id))
            {
                Collection.Add(org);
                Mapping.Add(org.Name, org);
            }

            Database.Upsert(org);
        }

        public void Remove(Organization org)
        {
            Database.Delete(org.Id);
            Mapping.Remove(org.Name);
            Collection.Remove(org);
        }
        
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<Organization>(Constants.cn_org);
            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Database = null;
            Collection.Clear();
            Mapping.Clear();
        }
        
        public Dictionary<string, Organization> Mapping { get; }
        public SourceList<Organization> Collection { get; }
        public ILiteCollection<Organization> Database { get; private set; }
    }
}