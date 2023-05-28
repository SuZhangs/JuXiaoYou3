using Acorisoft.Miga.Doc.Networks;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class ComposeService : DirectoryService, IDifferenceProvider
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
        
        public void Resolve(Transaction transaction)
        {
            if(transaction is not DifferenceTransaction dt)return;
            Compose compose;
            
            if (dt.Position == DifferencePosition.Compose)
            {
                if (dt.Difference == Difference.Added)
                {
                    Database.Upsert(DeserializeFromBase64<Compose>(dt.Base64));
                }
                else
                {
                    Database.Delete(dt.Id);
                }
            }
            else
            {
                compose = Database.FindById(dt.Id);
                
                if (dt.Position == DifferencePosition.Current)
                {
                    compose.Current = DeserializeFromBase64<ComposeVersion>(dt.Base64);
                }
                else if (dt.Position == DifferencePosition.Draft)
                {
                    if (dt.Difference == Difference.Added)
                    {
                        compose.Drafts.Add(DeserializeFromBase64<ComposeVersion>(dt.Base64));
                    }
                    else if (dt.Difference == Difference.Replace)
                    {
                        var item = compose.Drafts.FirstOrDefault(x => x.Id == dt.Id);
                        var content = DeserializeFromBase64<ComposeVersion>(dt.Base64);
                        
                        if(item is null) return;
                        
                        item.Content          = content.Content;
                        item.Hash             = content.Hash;
                        item.Name             = content.Name;
                        item.ModifiedDateTime = content.ModifiedDateTime;
                    }
                    else
                    {
                        var item = compose.Drafts.FirstOrDefault(x => x.Id == dt.Id);
                        compose.Drafts.Remove(item);
                    }
                }
                else
                {
                    compose = Database.FindById(dt.Id);
                
                    if (dt.Difference == Difference.Added)
                    {
                        compose.RecycleBin.Add(DeserializeFromBase64<ComposeVersion>(dt.Base64));
                    }
                    else if (dt.Difference == Difference.Replace)
                    {
                        var item = compose.RecycleBin.FirstOrDefault(x => x.Id == dt.Id);
                        var content = DeserializeFromBase64<ComposeVersion>(dt.Base64);
                        
                        if(item is null) return;
                        
                        item.Content          = content.Content;
                        item.Hash             = content.Hash;
                        item.Name             = content.Name;
                        item.ModifiedDateTime = content.ModifiedDateTime;
                    }
                    else
                    {
                        var item = compose.RecycleBin.FirstOrDefault(x => x.Id == dt.Id);
                        compose.RecycleBin.Remove(item);
                    }
                        
                }
            }
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DifferenceTransaction dt)return;
            
            Compose compose;
            
            if (dt.Position == DifferencePosition.Compose)
            {
                dt.Base64 = SerializeToBase64(Database.FindById(dt.Id));
            }
            else
            {
                compose = Database.FindById(dt.Id);
                
                if (dt.Position == DifferencePosition.Current)
                {
                    dt.Base64 = SerializeToBase64(compose.Current);
                }
                else if (dt.Position == DifferencePosition.Draft && dt.Difference != Difference.Removed)
                {
                    var item = compose.Drafts.FirstOrDefault(x => x.Id == dt.SubId);
                    dt.Base64 = SerializeToBase64(item);
                }
                else if(dt.Position == DifferencePosition.Recycle && dt.Difference != Difference.Removed)
                {
                    var item = compose.RecycleBin.FirstOrDefault(x => x.Id == dt.SubId);
                    dt.Base64 = SerializeToBase64(item);
                }
            }
        }
        
        public List<DifferenceDescription> GetDescriptions()
        {
            return Database.FindAll().Select(x =>
            {
                return new DifferenceDescription
                {
                    Id          = x.Id,
                    CurrentHash = x.Current.Hash,
                    RecycleBin  = x.RecycleBin.Select(y => new Tuple<string, string>(y.Id, y.Hash)).ToList(),
                    Draft       = x.Drafts.Select(y => new Tuple<string, string>(y.Id, y.Hash)).ToList(),
                };
            }).ToList();
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