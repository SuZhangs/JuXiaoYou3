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
                
                //
                // 添加角色
                AddCharacter(x);
                
                //
                // 添加角色
                AddMessageFromOperation(
                    new SocialMessage
                    {
                        Id       = ID.Get(),
                        MemberID = character.Id,
                        Type = MessageType.MemberJoin
                    });
            }
        }

        private void AddPlainTextMessageImpl(CharacterUI character)
        {
            if (character is null)
            {
                return;
            }
            
            //
            // 添加角色
            AddMessageFromOperation(
                new SocialMessage
                {
                    Id       = ID.Get(),
                    MemberID = character.Id,
                    Content = MessageContent,
                    Type = MessageType.PlainText
                });

            MessageContent = null;
        }
        
        public AsyncRelayCommand MemberJoinCommand { get; }
        public RelayCommand<CharacterUI> AddPlainTextMessageCommand { get; }
    }
}