namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{

    /// <summary>
    /// 时间戳消息
    /// </summary>
    public class TimestampMessage : SocialMessage
    {
        
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public string MemberID { get; init; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string RelativeString { get; set; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string Timestamp { get; set; }
    }
}