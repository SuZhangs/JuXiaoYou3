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

            foreach (var character in r.Value)
            {
                var member = character.Id;
                
                Channel.ChannelSource
                       .Members
                       .Add(member);

                var title = GetCharacterTitle(member, Channel.RoleMapping, Channel.TitleMapping, out var role);
                var name  = GetCharacterName(character, Channel.AliasMapping);
                var x     = new CharacterUI(title, name, role, character);
                
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

        private async Task MemberLeaveImpl()
        {
            var r = await CharacterPickerViewModel.MultiSelect(_characterUIMapper.Values
                                                                                 .Where(x => x.Id != OwnerID));

            if (!r.IsFinished)
            {
                return;
            }

            foreach (var character in r.Value)
            {
                var member = character.Id;
                var channel = Channel.ChannelSource;
                
                //
                // 删除成员ID
                channel.Members
                       .Remove(member);

                //
                // 添加或者更新离线头像
                if (channel.AvatarMapping.ContainsKey(member))
                {
                    channel.AvatarMapping[member] = character.Avatar;
                }
                else
                {
                    channel.AvatarMapping.TryAdd(member, channel.Avatar);

                }
                
                //
                // 添加或者更新离线角色
                if (channel.RoleMapping.ContainsKey(member))
                {
                    channel.RoleMapping[member] = character.Role;
                }
                else
                {
                    channel.RoleMapping.TryAdd(member, character.Role);

                }
                
                //
                // 添加或者更新离线头像ID
                if (channel.TitleMapping.ContainsKey(member))
                {
                    channel.TitleMapping[member] = character.Title;
                }
                else
                {
                    channel.TitleMapping.TryAdd(member, character.Title);

                }

                //
                // 添加角色
                RemoveCharacter(character);

                //
                // 添加角色
                AddMessageFromOperation(
                    new SocialMessage
                    {
                        Id       = ID.Get(),
                        MemberID = character.Id,
                        Type     = MessageType.MemberLeave
                    });
            }
        }

        private void SwitchImpl(CharacterUI character)
        {
            if (character is null ||
                ReferenceEquals(character, Speaker))
            {
                return;
            }

            Speaker = character;
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
        public AsyncRelayCommand MemberLeaveCommand { get; }
        public RelayCommand<CharacterUI> SwitchCommand { get; }
        public RelayCommand<CharacterUI> AddPlainTextMessageCommand { get; }
    }
}