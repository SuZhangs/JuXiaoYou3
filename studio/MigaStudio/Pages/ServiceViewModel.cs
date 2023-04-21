using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaStudio.Pages.Services;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class ServiceViewModelProxy : BindingProxy<ServiceViewModel>{}
    
    public class ServiceViewModel : InTabViewModel
    {
        protected override void Initialize()
        {
            CreateDialogFeature<MusicPlayerViewModel>(string.Empty, "__MusicPlayer", null);
            CreatePageFeature<ColorServiceViewModel>(string.Empty, "__ColorService", null);
            CreatePageFeature<RankServiceViewModel>(string.Empty, "__RankService", null);
            CreatePageFeature<CompareServiceViewModel>(string.Empty, "__CompareService", null);
        }
    }
}