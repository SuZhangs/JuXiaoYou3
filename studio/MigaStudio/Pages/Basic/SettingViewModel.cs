using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Resources;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class SettingViewModelProxy : BindingProxy<SettingViewModel>
    {
    }

    public partial class SettingViewModel : SettingViewModelBase
    {
        private readonly Subject<ValueTuple<long, long, long, EngineCounter[]>> _subject;

        private int _count;

        public SettingViewModel()
        {
            _count          = 0;
            
            DatabaseCounter = CreateDatabaseCounter();
            Application     = CreateApplicationCounter();
            Logs            = CreateLogCounter();
            Self            = CreateSelfCounter(Application, Logs);

            _subject = new Subject<(long, long,long, EngineCounter[])>();
            _subject.ObserveOn(Scheduler)
                    .Subscribe(OnDataSourceChanged);

            SystemSetting        = Xaml.Get<SystemSetting>();
            AdvancedSetting      = SystemSetting.AdvancedSetting;
            BasicAppSetting      = Xaml.Get<BasicAppSetting>();
            DatabaseManager      = Studio.DatabaseManager();
            Title                = Language.GetText(ConstantValues.PageName_Setting);
            Repositories         = new ObservableCollection<RepositoryCache>();

            
            RefreshCommand          = Command(ComputeDirectorySize);
            OpenCommand             = Command<FolderCounter>(OpenCounter);
            CompressLogsCommand = AsyncCommand(CompressLogsImpl);
            CloseDatabaseCommand    = AsyncCommand(CloseDatabaseImpl);
            
            ConfigureRegularSetting();
            ApprovalRequired = false;
        }

        private void OnRefresh()
        {
            var ss = SystemSetting.RepositorySetting;
            DatabaseProperty = Database.Get<DatabaseProperty>();

            CurrentRepository = ss.LastRepository;
            Repositories.AddMany(ss.Repositories, true);
            ComputeDirectorySize();
        }

        protected override void OnStart()
        {
            OnRefresh();
            base.OnStart();
        }

        protected override void OnResume()
        {
            OnRefresh();
        }

        private async Task CloseDatabaseImpl()
        {
            await DatabaseManager.CloseAsync();
            var controller = Controller.Context
                                       .ControllerMaps[AppViewModel.IdOfQuickStartController];
            Controller.Context
                      .SwitchController(controller);
        }

        private void ConfigureRegularSetting()
        {
            ComboBox<MainTheme>(ConstantValues.Setting_MainTheme, BasicAppSetting.Theme, MainThemeSetting, ConstantValues.Themes);
            ComboBox<DatabaseMode>(ConstantValues.Setting_DebugMode, DatabaseMode.Release, DatabaseModeSetting, ConstantValues.DebugMode);
            ComboBox<CultureArea>(ConstantValues.Setting_Language, BasicAppSetting.Language, LanguageSetting, ConstantValues.Languages);
            Slider(ConstantValues.Setting_AutoSavePeriod, AdvancedSetting.AutoSavePeriod, AutoSavePeriod, 5, 30);
        }

        private void NotifyApplyWhenRestart()
        {
            if (_count % 5 == 0)
            {
                this.SuccessfulNotification(SubSystemString.ApplyWhenRestart);
            }

            _count++;
        }

        private void AutoSavePeriod(int x)
        {
            if (x == AdvancedSetting.AutoSavePeriod)
            {
                return;
            }

            AdvancedSetting.AutoSavePeriod = Math.Clamp(x, 5, 30);
            Xaml.Get<AutoSaveService>()
                .Elapsed = x;
        }

        private void MainThemeSetting(MainTheme x)
        {
            if (x == BasicAppSetting.Theme)
            {
                return;
            }

            BasicAppSetting.Theme = x;
            NotifyApplyWhenRestart();
        }

        private void LanguageSetting(CultureArea x)
        {
            if (x == BasicAppSetting.Language)
            {
                return;
            }

            BasicAppSetting.Language = x;
            NotifyApplyWhenRestart();
        }

        private void DatabaseModeSetting(DatabaseMode x)
        {
            var setting = Xaml.Get<SystemSetting>()
                              .AdvancedSetting;
            if (x == setting.DebugMode)
            {
                return;
            }

            setting.DebugMode = x;
            NotifyApplyWhenRestart();
        }


        public sealed override void Stop()
        {
            ForestApp.SaveBasicSetting(BasicAppSetting);
            Xaml.Get<SystemSetting>()
                .Save();
            ApprovalRequired = false;
        }

        private DatabaseProperty _databaseProperty;
        private string           _currentRepository;

        /// <summary>
        /// 获取或设置 <see cref="CurrentRepository"/> 属性。
        /// </summary>
        public string CurrentRepository
        {
            get => _currentRepository;
            set => SetValue(ref _currentRepository, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="DatabaseProperty"/> 属性。
        /// </summary>
        public DatabaseProperty DatabaseProperty
        {
            get => _databaseProperty;
            private set => SetValue(ref _databaseProperty, value);
        }

        public AsyncRelayCommand CloseDatabaseCommand { get; }
        public ObservableCollection<RepositoryCache> Repositories { get; }
        public IDatabaseManager DatabaseManager { get; }
        public IDatabase Database => DatabaseUtilities.Database;
        protected BasicAppSetting BasicAppSetting { get; }
        protected AdvancedSettingModel AdvancedSetting { get; }
        protected SystemSetting SystemSetting { get; }

        public sealed override bool Uniqueness => true;
    }
}