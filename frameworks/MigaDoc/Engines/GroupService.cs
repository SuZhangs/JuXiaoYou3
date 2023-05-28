using Acorisoft.Miga.Doc.Groups;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class GroupService : StorageService, IDirectlyDifferenceProvider
    {
        public GroupService()
        {
            IsLazyMode = true;
        }

        public void Add(GroupingInformation information)
        {
            if(information is null)return;
            Database.Upsert(information);
        }

        public void Remove(GroupingInformation information)
        {
            if(information is null)return;
            Database.Delete(information.Id);
        }
        
        
        public void Remove(string information)
        {
            if(string.IsNullOrEmpty(information))return;
            Database.Delete(information);
        }
        
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Group} dt) return;
            Database.Upsert(DeserializeFromBase64<GroupingInformation>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Group} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }
        
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.Group, this);
        }
        

        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.Group
            }));
        }
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<GroupingInformation>(Constants.cn_group);
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Database = null;
        }
        
        public ILiteCollection<GroupingInformation> Database { get; private set; }
    }
}