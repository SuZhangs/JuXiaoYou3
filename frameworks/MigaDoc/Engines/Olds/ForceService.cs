using Acorisoft.Miga.Doc.Entities;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class ForceService : StorageService
    {
        private const string CollectionName = "forces";
        
        public ForceService()
        {
            Collection = new SourceList<Force>();
            IsLazyMode = true;
        }

        public void Add(Force entity)
        {
            if (entity is null)
            {
                return;
            }

            if (Database.ContainName(entity.Name))
            {
                Database.Update(entity);
            }
            else
            {
                Database.Insert(entity);
                Collection.Add(entity);
            }
        }

        public void Remove(Force entity)
        {
            if (entity is null)
            {
                return;
            }

            if (Database.Delete(entity.Id))
            {
                Collection.Remove(entity);
            }
        }
        
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<Force>(CollectionName);
            Collection.AddRange(Database.FindAll());
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Database = null;
            Collection.Clear();
        }
        
        public SourceList<Force> Collection { get; }
        public ILiteCollection<Force> Database { get; private set; }
    }
}