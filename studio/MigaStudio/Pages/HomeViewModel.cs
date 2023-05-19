﻿using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class HomeViewModel : InTabViewModel
    {
        private const string StartUp                = "global.startup";
        private const string Documents              = "global.documents";
        private const string Compose                = "global.compose";
        private const string Service                = "global.service";
        private const string Tools                  = "global.tools";
        private const string Inspiration            = "global.inspiration";
        private const string InspirationAndRelative = "__InspirationAndRelative";
        private const string Relationship           = "global.Relationship";
        private const string StoryboardSegments     = "global.StoryboardSegments";
        private const string Home                   = "__Home";

        public HomeViewModel()
        {
            GotoPageCommand = Command<Type>(GotoPageImpl);
        }

        protected override void Initialize()
        {
            CreateGalleryFeature<StartupViewModel>(StartUp, Home);
            CreateGalleryFeature<UniverseViewModel>(StartUp, "__Universe");
            CreateGalleryFeature<ComposeGalleryViewModel>(StartUp, Compose);

            //
            //
            CreateGalleryFeature<InspirationViewModel>(InspirationAndRelative, "global.inspiration");
            CreateGalleryFeature<StoryboardSegmentsViewModel>(InspirationAndRelative, StoryboardSegments);
            CreateGalleryFeature<ServiceViewModel>(InspirationAndRelative, Service);
            CreateGalleryFeature<RelationshipViewModel>(InspirationAndRelative, Relationship);

            //
            //

            //
            //
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Character", DocumentType.Character);
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Ability", DocumentType.Skill);
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Geography", DocumentType.Geography);
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Item", DocumentType.Item);
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Other", DocumentType.Other);
            
            //
            //
            CreateGalleryFeature<TemplateGalleryViewModel>(Tools, "text.TemplateGalleryViewModel");
            CreateGalleryFeature<TemplateEditorViewModel>(Tools, "text.TemplateEditorViewModel");
            CreateGalleryFeature<ToolsViewModel>(Tools, Tools);
        }

        public override void OnStart()
        {
            foreach (var feature in Features)
            {
                feature.Cache?.Start();
            }

            SelectedFeature = Features.FirstOrDefault(x => x.NameId == Home);
        }

        public override void Stop()
        {
            foreach (var feature in Features)
            {
                feature.Cache?.Stop();
            }
        }

        public override void Suspend()
        {
            foreach (var feature in Features)
            {
                feature.Cache?.Suspend();
            }

            base.Suspend();
        }

        protected override void OnResume()
        {
            foreach (var feature in Features)
            {
                feature.Cache?.Resume();
            }

            
        }


        private void GotoPageImpl(Type type)
        {
            Controller.New(type);
        }

        public sealed override bool Uniqueness => true;
        public sealed override bool Removable => false;

        public RelayCommand<Type> GotoPageCommand { get; }
    }
}