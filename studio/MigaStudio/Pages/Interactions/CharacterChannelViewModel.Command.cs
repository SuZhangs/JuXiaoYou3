using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private async Task MemberJoinImpl()
        {
            var hash = Characters.Select(x => x.Id)
                                 .ToHashSet();
            var r = await SubSystem.MultiSelectExclude(DocumentType.Character, hash);

            if (!r.IsFinished)
            {
                return;
            }

            var i = r.Value
                     .Select(GetCharacter);
            
            foreach (var character in i)
            {
                var member = new ChannelMember
                {
                    MemberID     = character.Id,
                    AliasMapping = new Dictionary<string, string>(),
                    Name         = character.Name,
                    Role         = MemberRole.Member,
                    Title        = string.Empty
                };
                
                Channel.ChannelSource
                       .Members
                       .Add(member);

                var x = new CharacterUI(member, character);
                Characters.Add(x);
                AddMessage(character, member, new MemberJoinMessage
                {
                    Id       = ID.Get(),
                    MemberID = character.Id,
                    Content = GetMemberJoinText(x)
                });
            }
        }
        
        public AsyncRelayCommand MemberJoinCommand { get; }
    }
}