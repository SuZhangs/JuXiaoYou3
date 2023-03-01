using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.MigaStudio.Resources;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class SettingViewModel : SettingViewModelBase
    {
        public SettingViewModel()
        {
            Title = Language.GetText(ConstantValues.PageName_Setting);
            ComboBox<MainTheme>(ConstantValues.PageName_Setting,)
        }
        
        public sealed override bool Uniqueness => true;
    }
}