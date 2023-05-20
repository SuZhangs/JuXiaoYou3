namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public enum CharacterRole
    {
        Owner,
        Manager,
        Member,
        Stranger
    }
    
    public class ChannelMember : StorageObject
    {
        public string Alias { get; set; }
        public string Title { get; set; }
        public CharacterRole Role { get; set; }
    }
    
    public class SocialChannel : StorageObject
    {
        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }
        
        /// <summary>
        /// 成员
        /// </summary>
        public List<ChannelMember> Member { get; init; }
        
        /// <summary>
        /// 消息
        /// </summary>
        public List<ChannelMessage> Messages { get; init; }
    }
}