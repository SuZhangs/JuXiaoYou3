using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class StartupViewModel : TabViewModel
    {
        public StartupViewModel()
        {
            SwitchControllerCommand =  Command<ControllerManifest>(SwitchControllerImpl);
        }
        protected override void OnResume()
        {
            RaiseUpdated(nameof(Controllers));
            base.OnResume();
        }

        private void SwitchControllerImpl(ControllerManifest manifest)
        {
            if (manifest is null)
            {
                return;
            }
            
            ModeSwitchViewModel.SwitchTo(Context, manifest);
        }

        public IEnumerable<ControllerManifest> Controllers
        {
            get
            {
                return Context.ControllerList
                              .Where(x => x.Value != AppViewModel.IdOfTabShellController);
            }
        }

        public GlobalStudioContext Context => Xaml.Get<AppViewModel>()
                                                  .Context;

        public RelayCommand<ControllerManifest> SwitchControllerCommand { get; }
    }
}