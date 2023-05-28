using Acorisoft.Miga.Doc.Keywords;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class RelationService : StorageService, IDirectlyDifferenceProvider
    {
        public RelationService()
        {
            IsLazyMode = true;
        }
        
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.Relationship, this);
        }
        
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Relationship} dt) return;
            Database.Upsert(DeserializeFromBase64<Relationship>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Relationship} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }
        
        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.Relationship
            }));
        }

        public void Add(Relationship relationship)
        {
            if (relationship is null)
            {
                return;
            }
            
            if (Database.Contains(relationship.Id))
            {
                Database.Update(relationship);
            }
            else
            {
                Database.Insert(relationship);
            }
        }

        public void Remove(Relationship relationship)
        {
            if (relationship is null)
            {
                return;
            }

            Database.Delete(relationship.Id);
        }
        
        public void Remove(RelCopy relationship)
        {
            if (relationship is null)
            {
                return;
            }

            Database.Delete(relationship.Id);
        }

        public void Remove(DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }

            Database.DeleteMany(x => x.Source.Id == index.Id  || x.Target.Id  == index.Id );
        }

        public Relationship GetRelationship(string sourceId, string targetId) => Database.FindOne(x => x.Source.Id == sourceId || x.Target.Id == targetId);

        public IEnumerable<Relationship> GetRelationships() => Database.FindAll();

        public IEnumerable<Relationship> GetRelationships(DocumentIndex index) => Database.Find(x => x.Source.Id == index.Id || x.Target.Id == index.Id);

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<Relationship>(Constants.cn_characterMapping);
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Database = null;
        }
        
        
        public ILiteCollection<Relationship> Database { get; private set; }
    }
}