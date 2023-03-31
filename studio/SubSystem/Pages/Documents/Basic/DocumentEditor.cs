using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Previews;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public abstract partial class DocumentEditorVMBase : TabViewModel
    {
        //
        // Fields
        //
        //                                                  ------------------
        private HeaderedSubView  _selectedSubView;    // SubViews
        private FrameworkElement _subView;            // ------------------
        private IPartOfDetail    _selectedDetailPart; // Detail
        private IPartOfDetailUI  _detailPartOfDetail; // ------------------
        private PartOfBasic      _basicPart;
        private Document         _document;           // Document
        private DocumentCache    _cache;              //------------------
        private PartOfModule     _selectedModulePart; // Module
        private bool             _dirtyState;


        protected DocumentEditorVMBase()
        {
            _sync                   = new object();
            _DataPartTrackerOfId    = new Dictionary<string, DataPart>(StringComparer.OrdinalIgnoreCase);
            ContentBlocks           = new ObservableCollection<ModuleBlockDataUI>();
            InternalSubViews        = new ObservableCollection<HeaderedSubView>();
            SubViews                = new ReadOnlyCollection<HeaderedSubView>(InternalSubViews);
            DetailParts             = new ObservableCollection<IPartOfDetail>();
            InvisibleDataParts      = new ObservableCollection<DataPart>();
            ModuleParts             = new ObservableCollection<PartOfModule>();
            PreviewBlocks           = new ObservableCollection<PreviewBlock>();
            _MetadataTrackerByName  = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _MetadataTrackerByIndex = new Dictionary<int, Metadata>();

            var dbMgr = Xaml.Get<IDatabaseManager>();
            DatabaseManager = dbMgr;
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();
            TemplateEngine  = dbMgr.GetEngine<TemplateEngine>();
            KeywordEngine   = dbMgr.GetEngine<KeywordEngine>();

            AddKeywordCommand    = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand = AsyncCommand<string>(RemoveKeywordImpl, x => !string.IsNullOrEmpty(x));
            Initialize();
        }

        private void Initialize()
        {
            CreateSubViews(InternalSubViews);
            SelectedSubView = InternalSubViews.FirstOrDefault();
        }

        protected static void AddSubView<TView>(ICollection<HeaderedSubView> collection, string id, bool caching = true) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Caching = caching,
                Name    = Language.GetText(id),
                Type    = typeof(TView)
            });
        }

        /// <summary>
        /// 创建子页面
        /// </summary>
        /// <param name="collection">集合</param>
        protected abstract void CreateSubViews(ICollection<HeaderedSubView> collection);

        #region OnStart

        private void LoadDocumentIntern()
        {
            foreach (var part in _document.Parts
                                          .Where(part => _DataPartTrackerOfId.TryAdd(part.Id, part)))
            {
                if (part is PartOfBasic pob)
                {
                    _basicPart = pob;
                }
                else if (part is PartOfModule pom)
                {
                    ModuleParts.Add(pom);
                }
                else if (part is IPartOfDetail pod)
                {
                    DetailParts.Add(pod);
                }
                else if (part is PartOfManifest)
                {
                    InvisibleDataParts.Add(part);
                }
            }

            CheckDataPart();
            TrackDataPartAndMetadata();
        }

        private void CheckDataPart()
        {
            if (_basicPart is null)
            {
                _basicPart = new PartOfBasic{ Buckets = new Dictionary<string, string>()};
                _document.Parts.Add(_basicPart);

                Name = _cache.Name;
            }
        }

        private void TrackDataPartAndMetadata()
        {
            foreach (var metadata in _basicPart.Buckets)
            {
                UpsertMetadata(metadata.Key, metadata.Value);
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

        private void CreateDocumentIntern()
        {
            var document = new Document
            {
                Id        = ID.Get(),
                Name      = _cache.Name,
                Version   = 1,
                Removable = true,
                Type      = Type,
                Parts     = new DataPartCollection(),
                Metas     = new MetadataCollection(),
            };

            //
            //
            _document = document;
            CreateDocumentWithManifest(document);
            OnCreateDocument(document);

            //
            // TODO: add to engine
        }

        private void CreateDocumentWithManifest(Document document)
        {
            var manifest = DatabaseManager.Database
                                          .CurrentValue
                                          .Get<ModuleManifestProperty>()
                                          .GetModuleManifest(Type);

            if ( Type != manifest?.Type)
            {
                return;
            }

            var iterators = manifest.Templates
                                    .Select(x => TemplateEngine.CreateModule(x));

            //
            //
            document.Parts.AddRange(iterators);
        }

        protected abstract void OnCreateDocument(Document document);

        protected override void OnStart(NavigationParameter parameter)
        {
            // TODO:
            _cache    = (DocumentCache)parameter.Index;
            _document = DocumentEngine.GetDocument(parameter.Id);
            Type      = _cache.Type;

            if (_document is null)
            {
                //
                // 创建文档
                CreateDocumentIntern();
            }

            // 加载文档
            LoadDocumentIntern();

            base.OnStart(parameter);
        }

        public override void OnStart()
        {
            SelectedDetailPart = DetailParts.FirstOrDefault();
            SelectedModulePart = ModuleParts.FirstOrDefault();
            base.OnStart();
        }

        #endregion

        #region SetDirtyState

        protected void SetDirtyState(bool state)
        {
            _dirtyState = state;

            if (state)
            {
                SetTitle(_document.Name, true);
            }

            ApprovalRequired = state;
        }

        #endregion
    }
}