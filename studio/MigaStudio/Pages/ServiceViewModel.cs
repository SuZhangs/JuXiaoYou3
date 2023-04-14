using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Pages.Services;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ServiceViewModelProxy : BindingProxy<ServiceViewModel>{}
    
    public class ServiceViewModel : TabViewModel
    {        
        private readonly Subject<ViewModelBase> _onStart;
        private               MainFeature            _selectedFeature;
        private               ViewModelBase          _selectedViewModel;
        
        public ServiceViewModel()
        {
            _onStart = new Subject<ViewModelBase>();
            _onStart.ObserveOn(Scheduler)
                    .Subscribe(x => SelectedViewModel = x)
                    .DisposeWith(Collector);
            Features          = new ObservableCollection<MainFeature>();
            RunFeatureCommand = AsyncCommand<MainFeature>(RunFeature);
            Initialize();
        }

        private void Initialize()
        {
            CreateDialogFeature<MusicPlayerViewModel>(string.Empty, "__MusicPlayer", null);
            CreateDialogFeature<ColorServiceViewModel>(string.Empty, "__ColorService", null);
            CreateDialogFeature<RankServiceViewModel>(string.Empty, "__RankService", null);
            CreateDialogFeature<CompareServiceViewModel>(string.Empty, "__CompareService", null);
        }
        
        private void CreateDialogFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = true,
                Parameter = e
            };
            Features.Add(f);
        }

        private void CreateGalleryFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog = true,
                Parameter = e
            };
            Features.Add(f);
        }
        
        private async Task RunFeature(MainFeature feature)
        {
            await MainFeature.Run(feature, DialogService, Controller, _onStart);
        }
        
        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<MainFeature> RunFeatureCommand { get; }

        public ObservableCollection<MainFeature> Features { get; init; }

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
    }
}