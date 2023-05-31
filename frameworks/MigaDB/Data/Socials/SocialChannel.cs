using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class ChannelMember
    {
        public string MemberID { get; init; }
        public string Name { get; init; }
        public Dictionary<string, string> AliasMapping { get; init; }
        public string Title { get; set; }
        public MemberRole Role { get; set; }
    }
    
    public class SocialChannel : StorageUIObject
    {
        private string _name;
        private string _avatar;
        private string _intro;

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        /// <summary>
        /// 所有成员
        /// </summary>
        public ObservableCollection<ChannelMember> Members { get; init; }
        
        /// <summary>
        /// 所有消息
        /// </summary>
        public ObservableCollection<SocialMessage> Messages { get; init; }
    }
}