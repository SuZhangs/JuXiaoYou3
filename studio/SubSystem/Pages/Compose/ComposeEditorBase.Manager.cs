using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    partial class ComposeEditorBase
    {
        protected override Compose GetDocumentById(string id)
        {
            return ComposeEngine.GetCompose(id);
        }

        protected override void PrepareOpeningDocument(ComposeCache cache, Compose document)
        {
            ActivateAllEngines();
            SynchronizeKeywords();
        }
        
        
        #region OnLoad

        protected override void FinishOpeningDocument(ComposeCache cache, Compose document)
        {
            AddMetadataWhenDocumentOpening();
        }


        private void AddMetadataWhenDocumentOpening()
        {
            // foreach (var metadata in BasicPart.Buckets)
            // {
            //     UpsertMetadataWithoutSave(metadata.Key, metadata.Value);
            // }
            //
            // foreach (var module in ModuleParts)
            // {
            //     foreach (var block in module.Blocks
            //                                 .Where(x => !string.IsNullOrEmpty(x.Metadata)))
            //     {
            //         AddMetadata(block.ExtractMetadata());
            //     }
            // }
        }
        
        protected override void IsDataPartExistence(Compose compose)
        {
            //
            // 检查当前打开的文档是否缺失指定的DataPart
            if (Markdown is null)
            {
                Markdown = new PartOfMarkdown();
                Document.Parts.Add(Markdown);
                AddDataPart(Markdown);
            }

            if (Album is null)
            {
                Album = new PartOfAlbum();
                Document.Parts.Add(Album);
                AddDataPart(Album);
            }
        }

        #endregion

        #region OnCreate

        protected override Compose CreateDocument()
        {
            var document = new Compose
            {
                Id        = Cache.Id,
                Name      = Cache.Name,
                Parts     = new DataPartCollection(),
                Metas     = new MetadataCollection(),
            };

            //
            //
            Document = document;
            OnCreateDocument(document);

            ComposeEngine.AddCompose(document);
            return document;
        }


        private static void CreateComposeFromManifest(Compose document)
        {
            document.Parts.Add(new PartOfMarkdown());
            document.Parts.Add(new PartOfAlbum());
        }
        #endregion

    }
}