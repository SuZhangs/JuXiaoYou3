﻿namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class SocialMessage : StorageObject
    {
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public string MemberID { get; init; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string RelativeString { get; set; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 发送的内容。
        /// </summary>
        public string Source { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public MessageType Type { get; init; }
    }

    public enum MessageType
    {
        Audio,
        File,
        Image,
        Gif,
        Video,
        RedPacket,
        PlainText,
        Recall,
        Call,
        Timestamp,
        Transfer,
        Muted,
        MemberJoin,
        MemberLeave,
    }
}