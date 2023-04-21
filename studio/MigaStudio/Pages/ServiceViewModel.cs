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