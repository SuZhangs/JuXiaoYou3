using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Pages.Services;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ToolsViewModelProxy : BindingProxy<ToolsViewModel>
    {
    }

    public class ToolsViewModel : TabViewModel
    {
        private readonly Subject<ViewModelBase> _onStart;
        private          MainFeature            _selectedFeature;
        private          ViewModelBase          _selectedViewModel;

        public ToolsViewModel()
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
            CreateDialogFeature<DirectoryManagerViewModel>(string.Empty, "__DirectoryStatistic", null);
            CreateDialogFeature<RepairToolViewModel>(string.Empty, "global.repair", null);
            CreateDialogFeature<KeywordViewModel>(string.Empty, "__Keywords", null);
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
                IsDialog  = true,
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