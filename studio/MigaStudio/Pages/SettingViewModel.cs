using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class SettingViewModel : TabViewModel
    {
        public SettingViewModel()
        {
            Title = "设置";
        }
        
        public sealed override bool Uniqueness => true;
    }
}