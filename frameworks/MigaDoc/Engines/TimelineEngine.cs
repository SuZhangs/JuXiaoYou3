using Newtonsoft.Json;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class TimelineEngine : DirectoryService, IDirectlyDifferenceProvider
    {
        private const string SubFolders             = "Timelines";
        private const string DocumentCollectionName = "timelines";
        private const string TimelineSetCollectionName = "timelineSets";

        public TimelineEngine() : base(SubFolders)
        {
            Collection = new SourceList<TimelineSet>();
            IsLazyMode = true;
        }
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Timeline} dt) return;
            Database.Upsert(DeserializeFromBase64<TimelineSet>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Timeline} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.Timeline, this);
        }
        

        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.Timeline
            }));
        }
        

        public void Add(TimelineSet set)
        {
            if (set is null)
            {
                return;
            }
            
            if (!Database.ContainName(set.Name))
            {
                Collection.Add(set);
                Database.Insert(set);
            }
            else
            {
                Database.Update(set);   
            }
        }

        public void Save()
        {
            Database.Upsert(Collection.Items);
        }

        public void Edit(TimelineSet set)
        {
            if (set is null)
            {
                return;
            }
            Database.Update(set);
        }
        
        public void Remove(TimelineSet set)
        {
            if (set is null)
            {
                return;
            }
            
            if (Database.Delete(set.Id))
            {
                Collection.Remove(set);
            }
        }

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            base.OnRepositoryOpening(context, property);

            if (context.Database.CollectionExists(DocumentCollectionName))
            {
                var database = context.Database.GetCollection<Timeline>(DocumentCollectionName);
                var list = database.FindAll().ToArray();
                var payload = JsonConvert.SerializeObject(list);
                var fileName = Path.Combine(Directory, "timeline.json");
                File.WriteAllText(fileName, payload);

                context.Database.DropCollection(DocumentCollectionName);
            }

            Database = context.Database.GetCollection<TimelineSet>(TimelineSetCollectionName);
            Collection.AddRange(Database.FindAll());

        }


        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            //
            //
            base.OnRepositoryClosing(context);

            Collection.Clear();
            Database = null;
        }

        public SourceList<TimelineSet> Collection { get; }
        public ILiteCollection<TimelineSet> Database { get; private set; }
    }
}