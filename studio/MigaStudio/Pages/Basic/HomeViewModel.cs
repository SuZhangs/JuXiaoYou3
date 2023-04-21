using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class HomeViewModel : TabViewModel
    {
        private const    string                 StartUp            = "global.startup";
        private const    string                 Documents          = "global.documents";
        private const    string                 Compose            = "global.compose";
        private const    string                 Service            = "global.service";
        private const    string                 Tools              = "global.tools";
        private const    string                 Inspiration        = "global.Inspiration";
        private const    string                 Relationship     = "global.Relationship";
        private const    string                 StoryboardSegments = "global.StoryboardSegments";
        private const    string                 Home               = "__Home";
        private readonly Subject<ViewModelBase> _onStart;

        private MainFeature   _selectedFeature;
        private ViewModelBase _selectedViewModel;

        public HomeViewModel()
        {
            _onStart = new Subject<ViewModelBase>();
            _onStart.ObserveOn(Scheduler)
                    .Subscribe(x => SelectedViewModel = x)
                    .DisposeWith(Collector);
            GotoPageCommand   = Command<Type>(GotoPageImpl);
            RunFeatureCommand = AsyncCommand<MainFeature>(RunFeature);
            Features          = new ObservableCollection<MainFeature>();
            Initialize();
        }

        private void Initialize()
        {
            CreateGalleryFeature<StartupViewModel>(StartUp, HomeEntityImpl);
            CreateGalleryFeature<UniverseViewModel>(StartUp, "__Universe"EntityImpl);
            CreateGalleryFeature<ComposeGalleryViewModel>(StartUp, ComposeEntityImpl);
            
            //
            //
            CreateGalleryFeature<InspirationViewModel>(Inspiration, Inspiration);
            CreateGalleryFeature<StoryboardSegmentsViewModel>(Inspiration, StoryboardSegmentsEntityImpl);
            
            //
            //
            CreateGalleryFeature<RelationshipViewModel>(Relationship, RelationshipEntityImpl);
            
            //
            //
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Character", DocumentType.Character);
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Ability", DocumentType.Ability);
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Geography", DocumentType.Geography);
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Item", DocumentType.Item);
            CreateGalleryFeature<DocumentGalleryViewModel>(Documents, "__Other", DocumentType.Other);
            CreateGalleryFeature<ServiceViewModel>(Tools, ServiceEntityImpl);
            CreateGalleryFeature<TemplateGalleryViewModel>(Tools, "text.TemplateGalleryViewModel"EntityImpl);
            CreateGalleryFeature<TemplateEditorViewModel>(Tools, "text.TemplateEditorViewModel"EntityImpl);
            CreateGalleryFeature<ToolsViewModel>(Tools, ToolsEntityImpl);
        }

        private void CreateGalleryFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsGallery = true,
                IsDialog  = false,
                Parameter = e
            };
            Features.Add(f);
        }
        
        private void CreatePageFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = false,
                IsGallery = false,
                Parameter = e
            };
            Features.Add(f);
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

        public override void Resume()
        {
            foreach (var feature in Features)
            {
                feature.Cache?.Resume();
            }

            base.Resume();
        }


        private async Task RunFeature(MainFeature feature)
        {
            await MainFeature.Run(feature, DialogService, Controller, _onStart);
        }

        private void GotoPageImpl(Type type)
        {
            Controller.New(type);
        }


        /// <summary>
        /// 获取或设置 <see cref="SelectedViewModel"/> 属性。
        /// </summary>
        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set => SetValue(ref _selectedViewModel, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedFeature"/> 属性。
        /// </summary>
        public MainFeature SelectedFeature
        {
            get => _selectedFeature;
            set
            {
                SetValue(ref _selectedFeature, value);
                RunFeature(value)
                    .GetAwaiter()
                    .GetResult();
            }
        }

        public ObservableCollection<MainFeature> Features { get; }

        public sealed override bool Uniqueness => true;
        public sealed override bool Removable => false;

        public RelayCommand<Type> GotoPageCommand { get; }
        public AsyncRelayCommand<MainFeature> RunFeatureCommand { get; }
    }
}