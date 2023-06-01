using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public abstract class InteractionViewModelBase : TabViewModel
    {
        public static CharacterUI GetCharacter(SocialChannel channel, SocialCharacter character)
        {
            var title = GetCharacterTitle(character.Id, channel.RoleMapping, channel.TitleMapping);
            var name  = GetCharacterName(character, channel.AliasMapping);
            return new CharacterUI(title, name, character);
        }
        
        public static CharacterUI GetCharacter(ChannelUI channel, SocialCharacter character)
        {
            var title = GetCharacterTitle(character.Id, channel.RoleMapping, channel.TitleMapping);
            var name  = GetCharacterName(character, channel.AliasMapping);
            return new CharacterUI(title, name, character);
        }
        
        public static string GetOwnerName() => Language.GetText("text.Interaction.Owner");
        public static string GetManagerName() => Language.GetText("text.Interaction.Manager");

        public static string GetCharacterTitle(string id, 
            IReadOnlyDictionary<string, MemberRole> roleMapping,
            IReadOnlyDictionary<string, string> titleMapping)
        {
            if (roleMapping.TryGetValue(id, out var role))
            {
                if (role == MemberRole.Manager)
                {
                    return GetManagerName();
                }

                if (role == MemberRole.Owner)
                {
                    return GetOwnerName();
                }

                if (role == MemberRole.Special)
                {
                    return titleMapping.TryGetValue(id, out var title) ? title : string.Empty;
                }
            }

            return string.Empty;
        }
        

        public static string GetCharacterName(SocialCharacter ch, Dictionary<string, string> aliasMapping)
        {
            return aliasMapping.TryGetValue(ch.Id, out var alias) ? alias : ch.Name;
        }

        public ChannelUI GetChannel(SocialChannel channel)
        {
            var characters = channel.Members
                                    .Select(x =>
                                    {
                                        var ch        = CharacterMapper[x];
                                        var title     = GetCharacterTitle(x, channel.RoleMapping, channel.TitleMapping);
                                        var name      = GetCharacterName(ch, channel.AliasMapping);
                                        var character = new CharacterUI(title, name, ch);
                                        return character;
                                    });

            //
            // 添加
            return new ChannelUI(channel, characters);
        }
        
        public Dictionary<string, SocialCharacter> CharacterMapper { get; internal set; }
    }
}