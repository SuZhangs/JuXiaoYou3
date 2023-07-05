using System.Collections.Generic;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class InteractionController : ShellCore
    {
        public InteractionController()
        {
            CharacterMapper = new Dictionary<string, SocialCharacter>(StringComparer.OrdinalIgnoreCase);
        }

        protected override void RequireStartupTabViewModel()
        {
            New<InteractionStartupViewModel>();
            New<CharacterChannelViewModel>();
        }

        protected override void OnCurrentViewModelChanged(ITabViewModel oldViewModel, ITabViewModel newViewModel)
        {

            if (newViewModel is InteractionViewModelBase ivmb)
            {
                ivmb.CharacterMapper = CharacterMapper;
            }
            
            base.OnCurrentViewModelChanged(oldViewModel, newViewModel);
        }
        
        /// <summary>
        /// 角色映射
        /// </summary>
        public Dictionary<string, SocialCharacter> CharacterMapper { get; }
        
        /// <summary>
        /// ID
        /// </summary>
        public sealed override string Id => AppViewModel.IdOfInteractionController;
    }
    
    
}