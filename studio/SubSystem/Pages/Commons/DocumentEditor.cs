﻿using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaDB.Data.Templates;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models.Presentations;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{

    public class DocumentEditorBaseViewModelProxy : BindingProxy<DocumentEditorBase>
    {
    }

    public abstract partial class DocumentEditorBase : BasicEditable
    {
        //
        // Fields
        //
        //                                                  
        // SubViews
        private   object           _selectedDetailPart;
        private   FrameworkElement _detailPartOfDetail;
        private   PartOfModule     _selectedModulePart;


        protected DocumentEditorBase()
        {
            _BlockTrackerOfId      = new Dictionary<string, ModuleBlock>(StringComparer.OrdinalIgnoreCase);

            ContentBlocks      = new ObservableCollection<ModuleBlockDataUI>();
            DetailParts        = new ObservableCollection<PartOfDetail>();
            InvisibleDataParts = new ObservableCollection<DataPart>();
            ModuleParts        = new ObservableCollection<PartOfModule>();
            Presentations      = new ObservableCollection<PresentationUI>();
            Preshapes          = new ObservableCollection<FrameworkElement>();

            var dbMgr = Studio.DatabaseManager();
            Xaml.Get<IAutoSaveService>()
                .Observable
                .ObserveOn(Scheduler)
                .Subscribe(_ => { Save(); })
                .DisposeWith(Collector);

            DatabaseManager = dbMgr;
            ImageEngine     = dbMgr.GetEngine<ImageEngine>();
            TemplateEngine  = dbMgr.GetEngine<TemplateEngine>();

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


            OverridePresentationCommand         = AsyncCommand(OverridePresentationImpl);
            unOverridePresentationCommand       = AsyncCommand(unOverridePresentationImpl);
            SynchronizePresentationCommand      = AsyncCommand(SynchronizePresentationImpl);
            AddPresentationCommand              = AsyncCommand(AddPresentationImpl);
            RemovePresentationCommand           = AsyncCommand<PresentationUI>(RemovePresentationImpl);
            RefreshPresentationCommand          = Command(RefreshPresentation);
            EditPresentationCommand             = AsyncCommand<PresentationUI>(EditPresentationImpl);
            ShiftUpPresentationCommand          = Command<PresentationUI>(ShiftUpPresentationImpl);
            ShiftDownPresentationCommand        = Command<PresentationUI>(ShiftDownPresentationImpl);
            ExportPresentationAsPdfCommand      = AsyncCommand(ExportPresentationAsPdfImpl);
            ExportPresentationAsPictureCommand  = AsyncCommand(ExportPresentationAsPictureImpl);
            ExportPresentationAsMarkdownCommand = AsyncCommand(ExportPresentationAsMarkdownImpl);
        }

        private void Initialize()
        {
            InitializeSubView();

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

        private void SynchronizeKeywords()
        {
            Keywords.AddMany(KeywordEngine.GetKeywords(Cache.Id), true);
        }

        protected override void PrepareOpeningDocument(DocumentCache cache, Document document)
        {
            Initialize();
            ActivateAllEngines();
        }

        protected override void OpeningDocument(DocumentCache cache, Document document)
        {
            SynchronizeKeywords();
        }

        protected override void FinishOpeningDocument(DocumentCache cache, Document document)
        {
        }

        protected override void OnStart()
        {
            SelectedDetailPart = DetailParts.FirstOrDefault();
            SelectedModulePart = ModuleParts.FirstOrDefault();
            ResetPresentation();
            base.OnStart();
        }
        protected override void OnResume()
        {
            SynchronizeKeywords();
            
        }

        public override void Suspend()
        {
            base.Suspend();
        }

        #endregion


       
        protected override void OnDirtyStateChanged(bool state)
        {
            SetTitle(Document.Name, state);
        }
        

        [NullCheck(UniTestLifetime.Constructor)]
        public ObservableCollection<DataPart> InvisibleDataParts { get; }



        [NullCheck(UniTestLifetime.Constructor)]
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public TemplateEngine TemplateEngine { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public ImageEngine ImageEngine { get; }
    }
}