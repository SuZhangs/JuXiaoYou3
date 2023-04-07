using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class HomeViewModelProxy : BindingProxy<HomeViewModel>
    {
    }

    public class HomeViewModel : TabViewModel
    {
        private readonly Subject<ViewModelBase> _onStart;

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
            const string StartUp   = "global.startup";
            const string Documents = "global.documents";
            const string Compose   = "global.compose";
            const string Service   = "global.service";
            const string Tools     = "global.tools";
            CreateGalleryFeature<UniverseViewModel>(StartUp, "__Home", null);
            CreateGalleryFeature<UniverseViewModel>(StartUp, "__Universe", null);
            CreateGalleryFeature<UniverseViewModel>(StartUp, Compose, null);
            CreateGalleryFeature<SimpleGalleryViewModel>(Documents, "__Character", DocumentType.Character);
            CreateGalleryFeature<SimpleGalleryViewModel>(Documents, "__Ability", DocumentType.Ability);
            CreateGalleryFeature<SimpleGalleryViewModel>(Documents, "__Geography", DocumentType.Geography);
            CreateGalleryFeature<SimpleGalleryViewModel>(Documents, "__Item", DocumentType.Item);
            CreateGalleryFeature<SimpleGalleryViewModel>(Documents, "__Other", DocumentType.Other);
            CreateGalleryFeature<ServiceViewModel>(Tools, Service, null);
            CreateGalleryFeature<TemplateGalleryViewModel>(Tools, "text.TemplateGalleryViewModel", null);
            CreateGalleryFeature<TemplateEditorViewModel>(Tools, "text.TemplateEditorViewModel", null);
            CreateGalleryFeature<ToolsViewModel>(Tools, Tools, null);
        }

        private void CreateGalleryFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel   = typeof(T),
                Parameter = e
            };
            Features.Add(f);
        }


        private async Task RunFeature(MainFeature feature)
        {
            if (feature is null)
            {
                return;
            }

            if (feature.IsDialog)
            {
                await DialogService().Dialog(Xaml.GetViewModel<IDialogViewModel>(feature.ViewModel));
                return;
            }

            
            var vm = Controller.Start(feature.ViewModel, feature.Parameter);
            _onStart.OnNext(vm);
        }

        private void GotoPageImpl(Type type)
        {
            Controller.New(type);
        }

        private async Task GotoDialogImpl(Type type)
        {
            await DialogService()
                .Dialog(Xaml.GetViewModel<IDialogViewModel>(type));
        }

        private MainFeature   _selectedFeature;
        private ViewModelBase _selectedViewModel;

        /// <summary>
        /// 获取或设置 <see cref="SelectedViewModel\"/> 属性。
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

        public ObservableCollection<MainFeature> Features { get; init; }

        public sealed override bool Uniqueness => true;
        public sealed override bool Removable => false;

        public RelayCommand<Type> GotoPageCommand { get; }
        public AsyncRelayCommand<MainFeature> RunFeatureCommand { get; }
    }
}