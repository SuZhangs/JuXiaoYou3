using System.Collections.Generic;
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


        private void AddMessage(SocialCharacter character, ChannelUI channel, SocialMessage message)
        {
            Channel.ChannelSource
                   .Messages
                   .Add(message);

            Channel.Messages
                   .Add(GetMessage(character, channel, message));
        }

        private ChatMessageUI GetMessage(SocialCharacter character, ChannelUI channel, SocialMessage message)
        {
            return message switch
            {
                PlainTextMessage ptm  => new PlainTextMessageUI(ptm, character, channel),
                MemberJoinMessage mjm => new MemberJoinMessageUI(mjm, _characterUIMapper),
                _                     => new TimestampMessageUI()
            };

            return new TimestampMessageUI();
        }

        public static string GetMemberJoinText(CharacterUI character)
        {
            return string.Format(Language.GetText("text.Interaction.MemberJoin"), character.Name);
        }
    }
}