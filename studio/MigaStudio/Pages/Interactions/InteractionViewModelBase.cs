using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public abstract class InteractionViewModelBase : TabViewModel
    {

        public static SocialCharacter GetCharacter(DocumentCache x)
        {
            return new SocialCharacter
            {
                Id     = x.Id,
                Name   = x.Name,
                Avatar = x.Avatar,
                Intro  = x.Intro
            };
        }
        public static string GetOwnerName() => Language.GetText("text.Interaction.Owner");
        public static string GetManagerName() => Language.GetText("text.Interaction.Manager");
        
        public Dictionary<string, SocialCharacter> CharacterMapper { get; internal set; }
    }
}