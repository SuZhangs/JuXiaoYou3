using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Resources;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class SettingViewModel : SettingViewModelBase
    {
        public SettingViewModel()
        {
            BasicAppSetting = Xaml.Get<BasicAppSetting>();
            Title           = Language.GetText(ConstantValues.PageName_Setting);

            ConfigureRegularSetting();
            ApprovalRequired = false;
        }

        private void ConfigureRegularSetting()
        {
            ComboBox<MainTheme>(ConstantValues.Setting_MainTheme, BasicAppSetting.Theme, MainThemeSetting, ConstantValues.Themes);
            ComboBox<DatabaseMode>(ConstantValues.Setting_DebugMode, DatabaseMode.Release, DatabaseModeSetting, ConstantValues.DebugMode);
            ComboBox<CultureArea>(ConstantValues.Setting_Language,
                BasicAppSetting.Language,
                x => { BasicAppSetting.Language = x; },
                ConstantValues.Languages);
        }

        private void MainThemeSetting(MainTheme x)
        {
            BasicAppSetting.Theme = x;
        }

        private void DatabaseModeSetting(DatabaseMode x)
        {
            Xaml.Get<SystemSetting>()
                .AdvancedSetting
                .DebugMode = x;
        }


        public sealed override void Stop()
        {
            ForestApp.SaveBasicSetting(BasicAppSetting);
            Xaml.Get<SystemSetting>()
                .Save();
            ApprovalRequired = false;
        }

        protected BasicAppSetting BasicAppSetting { get; }

        public sealed override bool Uniqueness => true;
    }
}