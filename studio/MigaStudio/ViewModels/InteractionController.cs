using Acorisoft.FutureGL.MigaStudio.Pages.Interactions;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class InteractionController : ShellCore
    {
        protected override void StartOverride()
        {
            base.StartOverride();
            New<CharacterContractViewModel>();
            New<CharacterChannelViewModel>();
        }

        protected override void RequireStartupTabViewModel()
        {
            New<InteractionStartupViewModel>();
        }
        
        public sealed override string Id => AppViewModel.IdOfInteractionController;

    }
    
    
}