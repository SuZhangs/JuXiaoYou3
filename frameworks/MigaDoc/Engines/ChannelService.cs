
namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class ChannelService : StorageService, IRefreshSupport, IDirectlyDifferenceProvider
    {
        public ChannelService()
        {
            IsLazyMode = true;
            Index      = new SourceList<ChannelIndex>();
        }

        public void Add(Channel channel)
        {
            if (channel is null)
            {
                return;
            }

            DB.Upsert(channel);
        }
        
        public void Add(ChannelIndex channel)
        {
            if (channel is null)
            {
                return;
            }

            if (!IndexDB.Contains(channel.Id))
            {
                Index.Add(channel);
            }
            IndexDB.Upsert(channel);
        }
        
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.Channel, this);
            context.Add(EntityID.ChannelIndex, this);
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction dt)return;
            dt.Base64 = dt.EntityId == EntityID.Channel ? 
                SerializeToBase64(DB.FindById(dt.Id)) : 
                SerializeToBase64(IndexDB.FindById(dt.Id));
        }

        public void Resolve(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction dt)return;
            
            if (dt.EntityId == EntityID.Channel)
            {
                DB.Upsert(DeserializeFromBase64<Channel>(dt.Base64));
            }
            else
            {
                
                IndexDB.Upsert(DeserializeFromBase64<ChannelIndex>(dt.Base64));
            }
        }
        

        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(IndexDB.FindAll().Select(x => new DirectlyDescription
            {
                Id = x.Id,
                EntityId = EntityID.ChannelIndex
            }));
            
            context.AddRange(DB.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.Channel
            }));
        }

        public Channel Open(string id)
        {
            return string.IsNullOrEmpty(id) ? null : DB.FindById(id);
        }

        public void Refresh()
        {
            Index.Clear();
            Index.AddRange(IndexDB.FindAll());
        }
        
        public void Remove(ChannelIndex index)
        {
            if (index is null)
            {
                return;
            }

            DB.Delete(index.Id);
            Index.Remove(index);
            IndexDB.Delete(index.Id);
        }
        
        public void Remove(DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }

            var hash = new HashSet<string>();
            foreach (var idx in IndexDB.Find(Query.EQ(nameof(ChannelIndex.Owner), index.Id)))
            {
                hash.Add(idx.Id);
            }

            DB.DeleteMany(x => hash.Contains(x.Id)); 
            IndexDB.DeleteMany(x => hash.Contains(x.Id));
        }


        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            DB      = context.Database.GetCollection<Channel>(Constants.cn_channel);
            IndexDB = context.Database.GetCollection<ChannelIndex>(Constants.cn_index_channel);
            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            IndexDB = null;
            DB = null;
        }

        public SourceList<ChannelIndex> Index { get; }
        public ILiteCollection<ChannelIndex> IndexDB { get; private set; }
        public ILiteCollection<Channel> DB { get; private set; }
    }
}