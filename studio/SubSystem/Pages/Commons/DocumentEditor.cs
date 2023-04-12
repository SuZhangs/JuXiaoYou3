﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models.Presentations;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class DetailPartSettingPlaceHolder : PartOfDetailPlaceHolder
    {
        public DetailPartSettingPlaceHolder()
        {
            Id = "__Detail_Setting";
        }
    }

    public class DocumentEditorBaseViewModelProxy : BindingProxy<DocumentEditorBase>
    {
    }

    public abstract partial class DocumentEditorBase : TabViewModel
    {
        //
        // Fields
        //
        //                                                  
        // SubViews
        private   FrameworkElement _subView;
        private   object           _selectedDetailPart;
        private   FrameworkElement _detailPartOfDetail;
        protected Document         Document;
        protected DocumentCache    Cache;
        private   PartOfModule     _selectedModulePart;


        protected DocumentEditorBase()
        {
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
            Presentations      = new ObservableCollection<PresentationUI>();

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
            NewDocumentCommand  = AsyncCommand(NewDocumentImpl);

            AddModulePartCommand       = AsyncCommand(AddModulePartImpl);
            RemoveModulePartCommand    = AsyncCommand<PartOfModule>(RemoveModulePartImpl, HasItem);
            UpgradeModulePartCommand   = Command(UpgradeModulePartImpl);
            ShiftUpModulePartCommand   = Command<PartOfModule>(ShiftUpModulePartImpl, x => NotFirstItem(ModuleParts, x));
            ShiftDownModulePartCommand = Command<PartOfModule>(ShiftDownModulePartImpl, x => NotLastItem(ModuleParts, x));


            AddDetailPartCommand       = AsyncCommand(AddDetailPartImpl);
            RemoveDetailPartCommand    = AsyncCommand<PartOfDetail>(RemoveDetailPartImpl, x => HasItem(x) && x.Removable);
            ShiftUpDetailPartCommand   = Command<PartOfDetail>(ShiftUpDetailPartImpl, x => NotFirstItem(DetailParts, x));
            ShiftDownDetailPartCommand = Command<PartOfDetail>(ShiftDownDetailPartImpl, x => NotLastItem(DetailParts, x));


            AddPresentationCommand              = AsyncCommand(AddPresentationImpl);
            RemovePresentationCommand           = AsyncCommand<PresentationUI>(RemovePresentationImpl);
            RefreshPresentationCommand          = Command(RefreshPresentationImp);
            EditPresentationCommand             = AsyncCommand<PresentationUI>(EditPresentationImpl);
            ShiftUpPresentationCommand          = Command<PresentationUI>(ShiftUpPresentationImpl);
            ShiftDownPresentationCommand        = Command<PresentationUI>(ShiftDownPresentationImpl);
            ExportPresentationAsPdfCommand      = AsyncCommand(ExportPresentationAsPdfImpl);
            ExportPresentationAsPictureCommand  = AsyncCommand(ExportPresentationAsPictureImpl);
            ExportPresentationAsMarkdownCommand = AsyncCommand(ExportPresentationAsMarkdownImpl);

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

        protected override void OnStart(Parameter parameter)
        {
            ActivateAllEngines();
            Cache    = (DocumentCache)parameter.Args[0];
            Document = DocumentEngine.GetDocument(Cache.Id);
            Type     = Cache.Type;

            Open();

            base.OnStart(parameter);
        }

        public override void OnStart()
        {
            SelectedDetailPart = DetailParts.FirstOrDefault();
            SelectedModulePart = ModuleParts.FirstOrDefault();
            Presentations.AddRange(PresentationPart.Blocks.Select(PresentationUI.GetUI), true);
            base.OnStart();
        }

        public override void Resume()
        {
            //
            // TODO:数据恢复
            base.Resume();
        }

        public override void Suspend()
        {
            base.Suspend();
        }

        #endregion


       
        protected override void OnDirtyStateChanged(bool state)
        {
            if (state)
            {
                SetTitle(Document.Name, true);
            }
        }
    }
}