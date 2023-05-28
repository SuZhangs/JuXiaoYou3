using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Acorisoft.Miga.Doc.Engines
{
    [Lazy]
    [GeneratedModules]
    public class DocumentService : DirectoryService, IDirectlyDifferenceProvider
    {
        private const string SubFolders             = "Documents";
        private const string CacheFolders           = "Caches";
        private const string FieldName              = "Name";

        public DocumentService() : base(SubFolders)
        {
        }

        public Document Open(string id)
        {
            return Database.Contains(id) ? Database.FindById(id) : null;
        }

        public void Add(Document document)
        {
            if (document is null)
            {
                return;
            }

            Database.Upsert(document);
        }

        public void Update(Document document, bool overwritten = true)
        {
            if (document is null)
            {
                return;
            }

            if (overwritten)
            {
                Database.Upsert(document);
                return;
            }

            if (Database.Contains(document.Id))
            {
                return;
            }

            Database.Insert(document);
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
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Document} dt) return;
            Database.Upsert(DeserializeFromBase64<Document>(dt.Base64));
        }

        public void Process(Transaction transaction)
        {
            if(transaction is not DirectlyTransaction{ EntityId: EntityID.Document} dt) return;
            dt.Base64 = SerializeToBase64(SerializeToBase64(Database.FindById(dt.Id)));
        }
        
        public void BuildService(IDictionary<EntityID, IDirectlyDifferenceProvider> context)
        {
            context.Add(EntityID.Document, this);
        }
        

        public void GetDescriptions(IList<DirectlyDescription> context)
        {
            context.AddRange(Database.FindAll().Select(x => new DirectlyDescription
            {
                Id       = x.Id,
                EntityId = EntityID.Document
            }));
        }
        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {

            Database = context.Database.GetCollection<Document>(Constants.cn_document);

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

        public ILiteCollection<Document> Database { get; private set; }
    }
}