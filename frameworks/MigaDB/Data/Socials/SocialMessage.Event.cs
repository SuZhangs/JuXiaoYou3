namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    /// <summary>
    /// 禁言消息
    /// </summary>
    public class MutedMessage : SocialMessage
    {
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public string MemberID { get; init; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string Content { get; set; }
    }
    
    /// <summary>
    /// 转账消息
    /// </summary>
    public class MemberJoinMessage : SocialMessage
    {
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public string MemberID { get; init; }
    }
    
    /// <summary>
    /// 转账消息
    /// </summary>
    public class MemberLeaveMessage : SocialMessage
    {
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