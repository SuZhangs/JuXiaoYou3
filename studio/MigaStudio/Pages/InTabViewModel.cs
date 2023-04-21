using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.Relationships;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public abstract class InTabViewModel : TabViewModel
    {
        private readonly Subject<ViewModelBase> _onStart;
        private          MainFeature            _selectedFeature;
        private          ViewModelBase          _selectedViewModel;

        protected InTabViewModel()
        {
            _onStart = new Subject<ViewModelBase>();
            _onStart.ObserveOn(Scheduler)
                    .Subscribe(x => SelectedViewModel = x)
                    .DisposeWith(Collector);
            Features          = new ObservableCollection<MainFeature>();
            RunFeatureCommand = AsyncCommand<MainFeature>(RunFeature);
            Init();
        }

        private void Init()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            CreatePageFeature<CharacterRelationshipViewModel>(string.Empty, "__CharacterRelationship", null);
            CreateDialogFeature<DirectoryManagerViewModel>(string.Empty, "__DirectoryStatistic", null);
            CreateDialogFeature<RepairToolViewModel>(string.Empty, "global.repair", null);
            CreatePageFeature<KeywordViewModel>(string.Empty, "__Keywords", null);
            CreatePageFeature<BookmarkViewModel>(string.Empty, "__Bookmark", null);
        }

        protected void CreateDialogFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = true,
                IsGallery = false,
                Parameter = e
            };
            Features.Add(f);
        }
        
        protected void CreateDialogFeature<T>(string group, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = string.Empty,
                ViewModel = typeof(T),
                IsDialog  = true,
                IsGallery = false,
                Parameter = e
            };
            Features.Add(f);
        }
        
        protected void CreateGalleryFeature<T>(string group, string name, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = name,
                ViewModel = typeof(T),
                IsDialog  = false,
                IsGallery = true,
                Parameter = e
            };
            Features.Add(f);
        }
        
        protected void CreateGalleryFeature<T>(string group, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = string.Empty,
                ViewModel = typeof(T),
                IsDialog  = false,
                IsGallery = true,
                Parameter = e
            };
            Features.Add(f);
        }
        
        protected void CreatePageFeature<T>(string group, params object[] e)
        {
            var f = new MainFeature
            {
                GroupId   = group,
                NameId    = string.Empty,
                ViewModel = typeof(T),
                IsDialog  = false,
                IsGallery = false,
                Parameter = e
            };
            Features.Add(f);
        }

        protected void CreatePageFeature<T>(string group, string name, params object[] e)
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

        private async Task RunFeature(MainFeature feature)
        {
            await MainFeature.Run(feature, DialogService, Controller, _onStart);
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<MainFeature> RunFeatureCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<MainFeature> Features { get; }

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
    }
}