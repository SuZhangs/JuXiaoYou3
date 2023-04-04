using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.Resources;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class SettingViewModel : SettingViewModelBase
    {
        public SettingViewModel()
        {
            BasicAppSetting = Xaml.Get<BasicAppSetting>();
            Title = Language.GetText(ConstantValues.PageName_Setting);
            
            ConfigureRegularSetting();
        }

        private void ConfigureRegularSetting()
        {
            
            ComboBox<MainTheme>(ConstantValues.Setting_MainTheme, 
                BasicAppSetting.Theme,
                x =>
                {
                    BasicAppSetting.Theme = x;
                    Save();
                },
                ConstantValues.Themes);
            
            ComboBox<CultureArea>(ConstantValues.Setting_Language,  
                BasicAppSetting.Language,
                x =>
                {
                    BasicAppSetting.Language = x;
                    Save();
                },
                ConstantValues.Languages);
        }

        private void Save()
        {
            ForestApp.SaveBasicSetting(BasicAppSetting);
        }

        public sealed override void Stop()
        {
        }

        protected BasicAppSetting BasicAppSetting { get; }

        public sealed override bool Uniqueness => true;
    }
}