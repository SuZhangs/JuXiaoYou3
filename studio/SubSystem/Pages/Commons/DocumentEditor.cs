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
using Acorisoft.FutureGL.MigaStudio.Models.Previews;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using DynamicData.Binding;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class DetailPartSettingPlaceHolder : PartOfDetailPlaceHolder
    {
        public DetailPartSettingPlaceHolder()
        {
            Id = "__Detail_Setting";
        }
    }

    public abstract partial class DocumentEditorBase : TabViewModel
    {
        //
        // Fields
        //
        //                                                  ------------------
            // SubViews
        private   FrameworkElement _subView;            // ------------------
        private   object           _selectedDetailPart; // Detail
        private   FrameworkElement _detailPartOfDetail; // ------------------
        protected Document         Document;            // Document
        protected DocumentCache    Cache;               //------------------
        private   PartOfModule     _selectedModulePart; // Module
        private   bool             _dirtyState;


        protected DocumentEditorBase()
        {
            _sync                  = new object();
            _DataPartTrackerOfId   = new Dictionary<string, DataPart>(StringComparer.OrdinalIgnoreCase);
            _MetadataTrackerByName = new Dictionary<string, MetadataIndexCache>(StringComparer.OrdinalIgnoreCase);
            _DataPartTrackerOfType = new Dictionary<Type, DataPart>();
            _BlockTrackerOfId      = new Dictionary<string, ModuleBlock>(StringComparer.OrdinalIgnoreCase);
            
            ContentBlocks      = new ObservableCollection<ModuleBlockDataUI>();
            InternalSubViews   = new ObservableCollection<SubViewBase>();
            SubViews           = new ReadOnlyCollection<SubViewBase>(InternalSubViews);
            DetailParts        = new ObservableCollection<PartOfDetail>();
            InvisibleDataParts = new ObservableCollection<DataPart>();
            ModuleParts        = new ObservableCollection<PartOfModule>();
            PreviewBlocks      = new ObservableCollection<PreviewBlockUI>();

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
            RemoveDetailPartCommand    = AsyncCommand<PartOfDetail>(RemoveDetailPartImpl, x => HasItem(x) && x.Removable);
            ShiftUpDetailPartCommand   = Command<PartOfDetail>(ShiftUpDetailPartImpl, x => NotFirstItem(DetailParts, x));
            ShiftDownDetailPartCommand = Command<PartOfDetail>(ShiftDownDetailPartImpl, x => NotLastItem(DetailParts, x));

            
            AddPreviewBlockCommand              = AsyncCommand(AddDetailPartImpl);
            RemovePreviewBlockCommand           = AsyncCommand<PreviewBlockUI>(RemovePreviewBlockImpl, HasItem);
            RefreshPreviewBlockCommand          = Command(RefreshPreviewBlockImp);
            ExportPreviewBlockAsPdfCommand      = AsyncCommand(ExportPreviewBlockAsPdfImpl);
            ExportPreviewBlockAsPictureCommand  = AsyncCommand(ExportPreviewBlockAsPictureImpl);
            ExportPreviewBlockAsMarkdownCommand = AsyncCommand(ExportPreviewBlockAsMarkdownImpl);
            
            AddKeywordCommand    = AsyncCommand(AddKeywordImpl);
            RemoveKeywordCommand = AsyncCommand<string>(RemoveKeywordImpl, x => !string.IsNullOrEmpty(x));
            
            Initialize();
        }

        private void Initialize()
        {
            CreateSubViews(InternalSubViews);
            SelectedSubView = InternalSubViews.FirstOrDefault();

            //
            // 添加绑定
            AddKeyBinding(ModifierKeys.Control, Key.S, Save);
        }


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
            Cache    = (DocumentCache)parameter.Index;
            Document = DocumentEngine.GetDocument(parameter.Id);
            Type     = Cache.Type;

            Open();

            base.OnStart(parameter);
        }

        public override void OnStart()
        {
            SelectedDetailPart = DetailParts.FirstOrDefault();
            SelectedModulePart = ModuleParts.FirstOrDefault();
            PreviewBlocks.AddRange(PreviewPart.Blocks.Select(PreviewBlockUI.GetUI), true);
            base.OnStart();
        }

        public override void Resume()
        {
            InternalSubViews.Clear();
            CreateSubViews(InternalSubViews);
            SelectedSubView    = InternalSubViews.FirstOrDefault();
            SelectedDetailPart = DetailParts.FirstOrDefault();
            SelectedModulePart = ModuleParts.FirstOrDefault();
            PreviewBlocks.AddRange(PreviewPart.Blocks.Select(PreviewBlockUI.GetUI), true);

            //
            // TODO:数据恢复
            base.Resume();
        }

        public override void Suspend()
        {
            InternalSubViews.Clear();
            PreviewBlocks.Clear();
            base.Suspend();
        }

        #endregion


        #region SetDirtyState

        public void SetDirtyState(bool state = true)
        {
            _dirtyState = state;

            if (state)
            {
                SetTitle(Document.Name, true);
            }

            ApprovalRequired = state;
        }

        #endregion
    }
}