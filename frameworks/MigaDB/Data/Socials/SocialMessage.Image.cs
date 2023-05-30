namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class ImageMessage : SocialMessage
    {
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public string MemberID { get; init; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string Source { get; set; }
    }
    
    public class GifMessage : SocialMessage
    {
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public string MemberID { get; init; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string Source { get; set; }
    }
}