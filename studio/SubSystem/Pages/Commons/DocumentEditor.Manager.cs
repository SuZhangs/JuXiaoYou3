using System.Linq;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    partial class DocumentEditorBase
    {
        [NullCheck(UniTestLifetime.Constructor)]
        private readonly Dictionary<string, MetadataIndexCache> _MetadataTrackerByName;
        
        
        private class MetadataIndexCache
        {
            private int _index;

            public Metadata Source { get; init; }

            public int Index
            {
                get => _index;
                init => _index = value;
            }

            /// <summary>
            /// 
            /// </summary>
            public string Value => Source.Value;

            public string this[MetadataCollection metadataCollection]
            {
                set
                {
                    if (Index < 0 || Index > metadataCollection.Count - 1)
                    {
                        _index = metadataCollection.Count;
                        metadataCollection.Add(Source);
                        Source.Value = value;
                    }
                    else
                    {
                        var outside = metadataCollection[Index];

                        if (outside.Value != Source.Value)
                        {
                            metadataCollection[Index].Value = value;
                        }

                        Source.Value = value;
                    }
                }
            }
        }

        #region Metadata

        
        protected void OnModuleBlockValueChanged(ModuleBlockDataUI dataUI, ModuleBlock block)
        {
            var metadataString = block.Metadata;
            
            //
            // ModuleBlockDataUI 已经实现了Value的Clamp和Fallback，不需要重新设置了
            // 这里只做Metadata的Add or Update
            if (block is GroupBlock)
            {
                return;
            }
            
            if (string.IsNullOrEmpty(metadataString))
            {
                return;
            }
            
            //
            //
            var metadata = block.ExtractMetadata();
            
            AddMetadata(metadata);
            
        }

        private Metadata GetMetadataById(string metadata)
        {
            return _MetadataTrackerByName.TryGetValue(metadata, out var v) ? v.Source : null;
        }
        
        private void AddMetadata(Metadata metadata)
        {
            if(metadata is null || string.IsNullOrEmpty(metadata.Name))
            {
                return;
            }

            if (_MetadataTrackerByName.TryGetValue(metadata.Name, out var metadataIndex))
            {
                metadataIndex[Document.Metas] = metadata.Value;
            }
            else
            {

                var checkedIndex = Document.Metas.Count;
                
                Document.Metas.Add(metadata);
                
                var index = new MetadataIndexCache
                {
                    Source = metadata,
                    Index  = checkedIndex
                };
                
                _MetadataTrackerByName.Add(metadata.Name, index);
            }
        }
        
        private void RemoveMetadata(string metadata)
        {
            if (_MetadataTrackerByName.TryGetValue(metadata, out var cache))
            {
                Document.Metas.RemoveAt(cache.Index);
                _MetadataTrackerByName.Remove(metadata);
            }
        }

        private void RemoveMetadata(Metadata metadata)
        {
            if (metadata is null || string.IsNullOrEmpty(metadata.Name))
            {
                return;
            }
            RemoveMetadata(metadata.Name);
        }

        private void ClearMetadata()
        {
            _MetadataTrackerByName.Clear();
            Document.Metas.Clear();
        }


        #endregion

        #region OnLoad

        private void LoadDocumentImpl()
        {
            AddDataPartWhenDocumentOpening();
            IsDataPartExistence();
            AddMetadataWhenDocumentOpening();
            GetPartOfPresentation();
        }

        private void AddDataPartWhenDocumentOpening()
        {
            foreach (var part in Document.Parts)
            {
                if (!string.IsNullOrEmpty(part.Id) &&
                    !_DataPartTrackerOfId.TryAdd(part.Id, part))
                {
                    Xaml.Get<ILogger>()
                        .Warn($"部件没有ID或者部件重复不予添加，部件ID：{part.Id}");
                    continue;
                }
                
                if (part is PartOfModule pom)
                {
                    ModuleParts.Add(pom);
                    AddBlock(pom.Name, pom.Blocks);
                    continue;
                }

                if (_DataPartTrackerOfType.TryAdd(part.GetType(), part))
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

        private void IsDataPartExistence()
        {
            //
            // 检查当前打开的文档是否缺失指定的DataPart

            if (BasicPart is null)
            {
                BasicPart = new PartOfBasic { Buckets = new Dictionary<string, string>() };
                
                //
                // Tracking
                Document.Parts.Add(BasicPart);
                _DataPartTrackerOfId.TryAdd(BasicPart.Id, BasicPart);
                _DataPartTrackerOfType.TryAdd(BasicPart.GetType(), BasicPart);
                
                //
                // Initialize
                Name   = Cache.Name;
                Gender = Language.GetText("global.DefaultGender");
            }
            
            //
            // 检查部件的缺失
            IsDataPartExistence(Document);
            DocumentUtilities.SynchronizeDocument(Cache, Document);
        }

        protected abstract void IsDataPartExistence(Document document);

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

        private void CreateDocumentImpl()
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

        protected abstract void OnCreateDocument(Document document);

        #endregion
        
        //
        // DocumentManager Part
        protected void Open()
        {
            if (Document is null)
            {
                //
                // 创建文档
                CreateDocumentImpl();
            }

            // 加载文档
            LoadDocumentImpl();
        }
    }
}