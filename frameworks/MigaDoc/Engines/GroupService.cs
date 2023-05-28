using Acorisoft.Miga.Doc.Groups;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class GroupService : StorageService
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