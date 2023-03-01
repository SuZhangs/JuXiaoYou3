using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.Resources;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

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
                x => BasicAppSetting.Theme = x,
                ConstantValues.Themes);
            
            ComboBox<CultureArea>(ConstantValues.Setting_Language,  
                BasicAppSetting.Language,
                x => BasicAppSetting.Language = x,
                ConstantValues.Languages);
        }

        public override void Stop()
        {
            
        }

        protected BasicAppSetting BasicAppSetting { get; }

        public sealed override bool Uniqueness => true;
    }
}