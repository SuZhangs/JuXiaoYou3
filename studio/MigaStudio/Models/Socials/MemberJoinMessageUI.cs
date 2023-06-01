using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class MemberJoinMessageUI : ChatMessageUI
    {
        public MemberJoinMessageUI(MemberJoinMessage message, Dictionary<string, CharacterUI> characterMapper)
        {
            MessageSource = message;
            MemberID      = message.MemberID;
            Content = CharacterChannelViewModel.GetMemberJoinText(characterMapper[MemberID]);
        }
        
        public MemberJoinMessage MessageSource { get; }
        
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public string MemberID { get; init; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string Content { get; set; }
    }
}