using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private void AddLatestSpeaker(CharacterUI character)
        {
            if (LatestSpeakers.Count > 9)
            {
                LatestSpeakers.RemoveAt(LatestSpeakers.Count - 1);
                LatestSpeakers.Insert(0, character);
            }
            else
            {
                LatestSpeakers.Add(character);
            }
        }

        private void AddMessageFromInitialize(SocialMessage message)
        {
            // var character = _characterUIMapper[message.MemberID];
            
            //
            //
            Channel.Messages
                   .Add(GetMessage(message));
        }

        private void AddMessageFromOperation(SocialMessage message)
        {
            Channel.ChannelSource
                   .Messages
                   .Add(message);
            //
            //
            Channel.Messages
                   .Add(GetMessage(message));
        }

        private void AddCharacter(CharacterUI character)
        {
            _characterUIMapper.TryAdd(character.Id, character);
            Characters.Add(character);
            Channel.Members
                   .Add(character);
        }
        
        private void RemoveCharacter(CharacterUI character)
        {
            if (Characters.Count <= 1)
            {
                return;
            }

            _characterUIMapper.Remove(character.Id);
            Characters.Remove(character);
            Channel.Members
                   .Remove(character);
        }

        private ChatMessageUI GetMessage(SocialMessage message)
        {
            //
            // 判断是否为已经退群的用户
            var wasDeleted = !_characterUIMapper.TryGetValue(message.MemberID, out var character);
            var id         = message.MemberID;

            // 如果没有退群，且为空则返回null
            if (!wasDeleted && character is null)
            {
                return null;
            }
            
            
            var avatar = wasDeleted ? Channel.AvatarMapping[id] : character.Avatar;
            var name   = wasDeleted ? Channel.AliasMapping[id] : character.Name;
            var role   = GetCharacterRole(id, Channel.RoleMapping);
            var title  = GetCharacterTitle(id ,role, Channel.AliasMapping);
            
            ChatMessageUI msg = message.Type switch
            {
                MessageType.PlainText => new PlainTextMessageUI(message, avatar, title, name, role),
                MessageType.MemberJoin => new MemberJoinMessageUI(message, name),
                _ => new TimestampMessageUI()
            };

            if (msg is UserMessageUI um)
            {
                um.IsSelf = Speaker is not null && Speaker.Id == um.MemberID;
            }

            return msg;
        }


        public static string GetMemberJoinText()
        {
            return Language.GetText("text.Interaction.MemberJoin");
        }
    }
}