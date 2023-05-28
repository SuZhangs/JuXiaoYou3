
namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class ChannelService : StorageService, IRefreshSupport
    {
        public ChannelService()
        {
            IsLazyMode = true;
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
            }
            IndexDB.Upsert(channel);
        }

        public Channel Open(string id)
        {
            return string.IsNullOrEmpty(id) ? null : DB.FindById(id);
        }

        public void Refresh()
        {
        }
        
        public void Remove(ChannelIndex index)
        {
            if (index is null)
            {
                return;
            }

            DB.Delete(index.Id);
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

        public ILiteCollection<ChannelIndex> IndexDB { get; private set; }
        public ILiteCollection<Channel> DB { get; private set; }
    }
}