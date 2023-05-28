using ComposeIndex = Acorisoft.Miga.Doc.Documents.ComposeIndex;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class ComposeIndexService : StorageService, IRefreshSupport
    {
        public ComposeIndexService()
        {
        }

        public void Add(ComposeIndex index)
        {
            if (index is null)
            {
                return;
            }

            if (!Database.Contains(index.Id))
            {
            }

            Database.Upsert(index);
        }


        public void Remove(ComposeIndex index)
        {          
            if (index is null)
            {
                return;
            }
            
            Database.Delete(index.Id);
        }

        public void Refresh()
        {
        }

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<ComposeIndex>(Constants.cn_index_compose);
            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Database = null;
        }

        public ILiteCollection<ComposeIndex> Database { get; private set; }
    }
}