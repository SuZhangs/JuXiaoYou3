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
            var r = await CharacterPickerViewModel.MultiSelectExclude(CharacterMapper.Values, hash);

            if (!r.IsFinished)
            {
                return;
            }

            var i = r.Value;
            
            foreach (var character in i)
            {
                var member = character.Id;
                
                Channel.ChannelSource
                       .Members
                       .Add(member);

                var title = GetCharacterTitle(member, Channel.RoleMapping, Channel.TitleMapping);
                var name  = GetCharacterName(character, Channel.AliasMapping);
                var x     = new CharacterUI(title, name, character);
                Characters.Add(x);
                AddMessage(
                    character,
                    Channel,
                    new MemberJoinMessage
                {
                    Id       = ID.Get(),
                    MemberID = character.Id
                });
            }
        }
        
        public AsyncRelayCommand MemberJoinCommand { get; }
    }
}