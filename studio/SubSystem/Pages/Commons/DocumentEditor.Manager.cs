using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    partial class DocumentEditorBase
    {

        #region OnLoad


        protected override void FinishOpeningDocument(DocumentCache cache, Document document)
        {
            AddMetadataWhenDocumentOpening();
            GetPartOfPresentation();
        }

        protected override bool OnDataPartAddingBefore(DataPart part)
        {
            if (part is PartOfModule pom)
            {
                ModuleParts.Add(pom);
                AddBlock(pom.Name, pom.Blocks);
                return true;
            }

            return false;
        }

        protected override void OnDataPartAddingAfter(DataPart part)
        {
            if (DataPartTrackerOfType.TryAdd(part.GetType(), part))
            {
                if (part is PartOfBasic pob)
                {
                    BasicPart = pob;
                }
                else if (part is PartOfDetail pod)
                {
                    DetailParts.Add(pod);
                }
                else if (part is PartOfPresentation pop)
                {
                    //
                    // 优先覆盖
                    PresentationPart           = pop;
                    IsOverridePresentationPart = true;
                }
                else if (part is PartOfManifest)
                {
                    InvisibleDataParts.Add(part);
                }
            }
        }

        private void AddMetadataWhenDocumentOpening()
        {
            foreach (var metadata in BasicPart.Buckets)
            {
                UpsertMetadataWithoutSave(metadata.Key, metadata.Value);
            }

            foreach (var module in ModuleParts)
            {
                foreach (var block in module.Blocks
                                            .Where(x => !string.IsNullOrEmpty(x.Metadata)))
                {
                    AddMetadata(block.ExtractMetadata());
                }
            }
        }
        
        protected sealed override void IsDataPartExistence(Document document)
        {
            //
            // 检查当前打开的文档是否缺失指定的DataPart

            if (BasicPart is null)
            {
                BasicPart = new PartOfBasic { Buckets = new Dictionary<string, string>() };
                
                //
                // Tracking
                Document.Parts.Add(BasicPart);
                DataPartTrackerOfId.TryAdd(BasicPart.Id, BasicPart);
                DataPartTrackerOfType.TryAdd(BasicPart.GetType(), BasicPart);
                
                //
                // Initialize
                Name   = Cache.Name;
                Gender = Language.GetText("global.DefaultGender");
            }
            
            DocumentUtilities.SynchronizeDocument(Cache, Document);
            IsDataPartExistenceOverride(Document);
        }

        protected abstract void IsDataPartExistenceOverride(Document document);

        private void GetPartOfPresentation()
        {
            if (PresentationPart is null)
            {
                //
                // 打开 PresentationPart
                var db = Studio.DatabaseManager()
                             .Database
                             .CurrentValue;

                PresentationPart           = GetPresentationPreset(db, Type);
                IsOverridePresentationPart = false;
            }
        }

        #endregion

        #region OnCreate

        protected override Document CreateDocument()
        {
            var document = new Document
            {
                Id        = Cache.Id,
                Name      = Cache.Name,
                Version   = 1,
                Removable = true,
                Type      = Type,
                Parts     = new DataPartCollection(),
                Metas     = new MetadataCollection(),
            };

            //
            //
            Document = document;
            CreateDocumentFromManifest(document);
            OnCreateDocument(document);

            DocumentEngine.AddDocument(document);
            return document;
        }

        private void CreateDocumentFromManifest(Document document)
        {
            var manifest = Studio.Database()
                                 .Get<PresetProperty>()
                                 .GetModulePreset(Type);

            if (Type != manifest?.Type)
            {
                return;
            }

            var iterators = manifest.Templates
                                    .Select(x => TemplateEngine.CreateModule(x));

            //
            //
            document.Parts.AddMany(iterators);
        }

        #endregion

        protected override Document GetDocumentById(string id)
        {
            return DocumentEngine.GetDocument(id);
        }
    }
}