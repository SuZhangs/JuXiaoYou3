using Acorisoft.Miga.Doc.Entities;
using Conversation = Acorisoft.Miga.Doc.Entities.Conversation;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class TelegramService : StorageService, IRefreshSupport, IDirectlyDifferenceProvider
    {
        public TelegramService()
        {
            Collection = new SourceList<Conversation>();
            IsLazyMode = true;
        }
        
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.Conversation, this);
        }
        
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Conversation} dt) return;
            Database.Upsert(DeserializeFromBase64<Conversation>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Conversation} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }
        
        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.Conversation
            }));
        }
        
        public void Add(Conversation msg)
        {
            if (msg is null)
            {
                return;
            }

            if (!Database.Contains(msg.Id))
            {
                Collection.Add(msg);
            }
            Database.Upsert(msg);
            
        }
        
        
        public void Remove(Conversation msg)
        {
            if (msg is null)
            {
                return;
            }
            Database.Delete(msg.Id);
            Collection.Remove(msg);
        }
        
        public void Remove(DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }

            Database.DeleteMany(x => x.Index.Id == index.Id);
            Refresh();
        }

        public void Refresh()
        {
            Collection.Clear();
            Collection.AddRange(Database.Include(x => x.Index).FindAll());
        }


        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            Database = context.Database.GetCollection<Conversation>(Constants.cn_message);
            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            Collection.Clear();
            Database = null;
        }

        public SourceList<Conversation> Collection { get; }
        public ILiteCollection<Conversation> Database { get; private set; }
    }
}