using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class MemberJoinMessageUI : ChatMessageUI
    {
        public MemberJoinMessageUI(SocialMessage message, string name)
        {
            MessageSource = message;
            MemberID      = message.MemberID;
            Name    = name;
            WelcomeContent = CharacterChannelViewModel.GetMemberJoinText();
        }
        
        public SocialMessage MessageSource { get; }
        
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public string MemberID { get; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string WelcomeContent { get; }
        
        public string Name { get; }
    }
}