using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
using DynamicData.Binding;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public class DocumentEditorViewModelProxy : BindingProxy<DocumentEditorVMBase>
    {
    }

    public abstract partial class DocumentEditorVMBase : TabViewModel
    {
        //
        // Fields
        //
        //                                                  ------------------
        private HeaderedSubView  _selectedSubView;    // SubViews
        private FrameworkElement _subView;            // ------------------
        private object    _selectedDetailPart; // Detail
        private FrameworkElement _detailPartOfDetail; // ------------------
        private PartOfBasic      _basicPart;
        private Document         _document;           // Document
        private DocumentCache    _cache;              //------------------
        private PartOfModule     _selectedModulePart; // Module
        private bool             _dirtyState;


        protected DocumentEditorVMBase()
        {
            _sync                  = new object();
            _DataPartTrackerOfId   = new Dictionary<string, DataPart>(StringComparer.OrdinalIgnoreCase);
            ContentBlocks          = new ObservableCollection<ModuleBlockDataUI>();
            InternalSubViews       = new ObservableCollection<HeaderedSubView>();
            SubViews               = new ReadOnlyCollection<HeaderedSubView>(InternalSubViews);
            DetailParts            = new ObservableCollection<IPartOfDetail>();
            InvisibleDataParts     = new ObservableCollection<DataPart>();
            ModuleParts            = new ObservableCollection<PartOfModule>();
            PreviewBlocks          = new ObservableCollection<PreviewBlock>();
            _MetadataTrackerByName = new Dictionary<string, MetadataIndexCache>(StringComparer.OrdinalIgnoreCase);

            var dbMgr = Xaml.Get<IDatabaseManager>();
            Xaml.Get<IAutoSaveService>()
                .Observable
                .ObserveOn(Scheduler)
                .Subscribe(_ => { Save(); })
                .DisposeWith(Collector);

            DatabaseManager = dbMgr;
            DocumentEngine  = dbMgr.GetEngine<DocumentEngine>();
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();
            TemplateEngine  = dbMgr.GetEngine<TemplateEngine>();
            KeywordEngine   = dbMgr.GetEngine<KeywordEngine>();

            ChangeAvatarCommand = AsyncCommand(ChangeAvatarImpl);
            SaveDocumentCommand = Command(Save);

            AddModulePartCommand       = AsyncCommand(AddModulePartImpl);
            RemoveModulePartCommand    = AsyncCommand<PartOfModule>(RemoveModulePartImpl, HasItem);
            UpgradeModulePartCommand   = Command(UpgradeModulePartImpl);
            ShiftUpModulePartCommand   = Command<PartOfModule>(ShiftUpModulePartImpl, x => NotFirstItem(ModuleParts, x));
            ShiftDownModulePartCommand = Command<PartOfModule>(ShiftDownModulePartImpl, x => NotLastItem(ModuleParts, x));


            AddDetailPartCommand       = AsyncCommand(AddDetailPartImpl);
            RemoveDetailPartCommand    = AsyncCommand<IPartOfDetail>(RemoveDetailPartImpl, HasItem);
            ShiftUpDetailPartCommand   = Command<IPartOfDetail>(ShiftUpDetailPartImpl, x => NotFirstItem(DetailParts, x));
            ShiftDownDetailPartCommand = Command<IPartOfDetail>(ShiftDownDetailPartImpl, x => NotFirstItem(DetailParts, x));

            AddKeywordCommand    = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand = AsyncCommand<string>(RemoveKeywordImpl, x => !string.IsNullOrEmpty(x));
            Initialize();
        }

        private bool HasModulePart(PartOfModule module) => module is not null;
        private bool NotLastModulePart(PartOfModule module) => module is not null && ModuleParts.IndexOf(module) < ModuleParts.Count - 1;
        private bool NotFirstModulePart(PartOfModule module) => module is not null && ModuleParts.IndexOf(module) > 0;

        private void Initialize()
        {
            CreateSubViews(InternalSubViews);
            SelectedSubView = InternalSubViews.FirstOrDefault();
            
            //
            // 添加绑定
            AddKeyBinding(ModifierKeys.Control, Key.S, Save);
        }

        protected static void AddSubView<TView>(ICollection<HeaderedSubView> collection, string id) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Name = Language.GetText(id),
                Type = typeof(TView)
            });
        }

        /// <summary>
        /// 创建子页面
        /// </summary>
        /// <param name="collection">集合</param>
        protected abstract void CreateSubViews(ICollection<HeaderedSubView> collection);

        #region OnLoad

        private void LoadDocumentImpl()
        {
            AddDataPart();
            AddBasicPart();
            AddMetadata();
            MaintainDetailPart();
            Reorder();
        }

        private void AddDataPart()
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
        }

        private void AddBasicPart()
        {
            if (_basicPart is null)
            {
                _basicPart = new PartOfBasic { Buckets = new Dictionary<string, string>() };
                _document.Parts.Add(_basicPart);
                Name   = _cache.Name;
                Gender = Language.GetText("global.DefaultGender");
            }
        }

        private void AddMetadata()
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

        private void MaintainDetailPart()
        {
        }

        private void Reorder()
        {
        }

        #endregion

        #region OnCreate

        private void CreateDocumentImpl()
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
            CreateDocumentFromManifest(document);
            OnCreateDocument(document);

            DocumentEngine.AddDocument(document);
        }

        private void CreateDocumentFromManifest(Document document)
        {
            var manifest = DatabaseManager.Database
                                          .CurrentValue
                                          .Get<ModuleManifestProperty>()
                                          .GetModuleManifest(Type);

            if (Type != manifest?.Type)
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

        #endregion

        #region OnStart

        private void ActivateAllEngines()
        {
            var engines = new DataEngine[]
            {
                TemplateEngine,
                ImageEngine,
                DocumentEngine,
                KeywordEngine,
            };

            foreach (var engine in engines)
            {
                if (!engine.Activated)
                {
                    engine.Activate();
                }
            }
        }

        protected override void OnStart(NavigationParameter parameter)
        {
            ActivateAllEngines();
            _cache    = (DocumentCache)parameter.Index;
            _document = DocumentEngine.GetDocument(parameter.Id);
            Type      = _cache.Type;

            if (_document is null)
            {
                //
                // 创建文档
                CreateDocumentImpl();
            }

            // 加载文档
            LoadDocumentImpl();

            base.OnStart(parameter);
        }

        public override void OnStart()
        {
            SelectedDetailPart = DetailParts.FirstOrDefault();
            SelectedModulePart = ModuleParts.FirstOrDefault();
            base.OnStart();
        }

        public override void Resume()
        {
            InternalSubViews.Clear();
            CreateSubViews(InternalSubViews);
            SelectedSubView    = InternalSubViews.FirstOrDefault();
            SelectedDetailPart = DetailParts.FirstOrDefault();
            SelectedModulePart = ModuleParts.FirstOrDefault();
            
            //
            // TODO:数据恢复
            
            base.Resume();
        }

        public override void Suspend()
        {
            InternalSubViews.Clear();
            base.Suspend();
        }

        #endregion

        #region SetDirtyState

        public void SetDirtyState(bool state = true)
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