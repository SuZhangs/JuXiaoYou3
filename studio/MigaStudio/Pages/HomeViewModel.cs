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
            _onStart          = new Subject<ViewModelBase>();
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
            CreateGalleryFeature<UniverseViewModel>("global.universe", "__Universe", null);
            CreateDocumentGalleryFeature<DocumentGalleryViewModel>("__Character", DocumentType.Character);
            CreateDocumentGalleryFeature<DocumentGalleryViewModel>("__Ability", DocumentType.Ability);
            CreateDocumentGalleryFeature<DocumentGalleryViewModel>("__Geography", DocumentType.Geography);
            CreateDocumentGalleryFeature<DocumentGalleryViewModel>("__Item", DocumentType.Item);
            CreateDocumentGalleryFeature<DocumentGalleryViewModel>("__Other", DocumentType.Other);
            CreateGalleryFeature<UniverseViewModel>("global.compose", "global.compose", null);
            CreateGalleryFeature<UniverseViewModel>("global.service", "global.service", null);
            CreateGalleryFeature<UniverseViewModel>("global.universe", "global.universe", null);
        }

        private void CreateGalleryFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                IsGallery = true,
                Gallery   = typeof(T),
                Parameter = e
            };
            Features.Add(f);
        }

        private void CreateDocumentGalleryFeature<T>(string name, DocumentType e)
        {
            CreateGalleryFeature<T>("global.documents", name, e);
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

            var vm = feature.IsGallery ? feature.Gallery : feature.ViewModel;
            var vm1 = Controller.Start(vm, feature.Parameter);
            _onStart.OnNext(vm1);
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

        private MainFeature _selectedFeature;
        private ViewModelBase      _selectedViewModel;

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