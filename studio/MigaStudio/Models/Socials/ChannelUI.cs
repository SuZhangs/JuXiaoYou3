using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class ChannelUI : ObservableObject
    {
        public ChannelUI(SocialChannel channel)
        {
            ChannelSource = channel ?? throw new ArgumentNullException(nameof(channel));
        }
        
        public SocialChannel ChannelSource { get;  }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => ChannelSource.Intro;
            set
            {
                ChannelSource.Intro = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => ChannelSource.Avatar;
            set
            {
                ChannelSource.Avatar = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => ChannelSource.Name;
            set
            {
                ChannelSource.Name = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 所有成员
        /// </summary>
        public ObservableCollection<CharacterUI> Members { get; init; }
        
        /// <summary>
        /// 所有消息
        /// </summary>
        public ObservableCollection<ChatMessageUI> Messages { get; init; }
    }
}