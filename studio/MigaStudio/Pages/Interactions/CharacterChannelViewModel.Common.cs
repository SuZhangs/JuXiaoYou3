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


        private void AddMessage(SocialCharacter character, ChannelMember member, SocialMessage message)
        {
            Channel.ChannelSource
                   .Messages
                   .Add(message);

            Channel.Messages
                   .Add(GetMessage(character, member, message));
        }

        private static ChatMessageUI GetMessage(SocialCharacter character, ChannelMember member, SocialMessage message)
        {
            if (message is PlainTextMessage ptm)
            {
                return new PlainTextMessageUI(ptm, character, member);
            }


            return new TimestampMessageUI();
        }

        private static string GetMemberJoinText(CharacterUI character)
        {
            return string.Format(Language.GetText("text.Interaction.MemberJoin"), character.Name);
        }
    }
}