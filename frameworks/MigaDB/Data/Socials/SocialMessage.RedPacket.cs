namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    /// <summary>
    /// 红包消息
    /// </summary>
    public class RedPacketMessage : SocialMessage
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