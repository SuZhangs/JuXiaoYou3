

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class ComposeService : DirectoryService
    {
        private const string SubFolders   = "Compose";
        private const string CacheFolders = "Caches";
        private const string FieldName    = "Name";

        public ComposeService() : base(SubFolders)
        {
        }

        public Compose Open(string id)
        {
            return Database.Contains(id) ? Database.FindById(id) : null;
        }

        public void Add(Compose compose)
        {
            if (compose is null)
            {
                return;
            }

            Database.Upsert(compose);
        }

        public void Update(Compose compose, bool overwritten = true)
        {
            if (compose is null)
            {
                return;
            }

            if (overwritten)
            {
                Database.Upsert(compose);
                return;
            }

            if (Database.Contains(compose.Id))
            {
                return;
            }

            Database.Insert(compose);
        }

        public void Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            Database.Delete(id);
        }
        

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {

            Database = context.Database.GetCollection<Compose>(Constants.cn_compose);

            //
            //
            base.OnRepositoryOpening(context, property);
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            //
            //
            base.OnRepositoryClosing(context);
            Database = null;
        }

        public ILiteCollection<Compose> Database { get; private set; }
    }
}