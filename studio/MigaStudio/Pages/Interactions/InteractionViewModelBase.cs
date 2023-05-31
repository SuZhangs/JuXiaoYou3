using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public abstract class InteractionViewModelBase : TabViewModel
    {

        public static string GetOwnerName() => Language.GetText("text.Interaction.Owner");
        public static string GetManagerName() => Language.GetText("text.Interaction.Manager");
        
        public Dictionary<string, SocialCharacter> CharacterMapper { get; internal set; }
    }
}