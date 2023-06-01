using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class MemberJoinMessageUI
    {
        public MemberJoinMessageUI(MemberJoinMessage message, Dictionary<string, SocialCharacter> characterMapper)
        {
            MessageSource = message;
            MemberID      = message.MemberID;
            Content       = characterMapper.TryGetValue(MemberID, out var ch) ? ch.Name : string.Empty;
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