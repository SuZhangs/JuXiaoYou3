using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class ChannelUI : ObservableObject
    {
        public ChannelUI(SocialChannel channel, IEnumerable<CharacterUI> characters)
        {
            ChannelSource = channel ?? throw new ArgumentNullException(nameof(channel));
            Members       = new ObservableCollection<CharacterUI>(characters);
            Messages      = new ObservableCollection<ChatMessageUI>();
        }

        public string Id => ChannelSource.Id;
        
        public SocialChannel ChannelSource { get;  }

        /// <summary>
        /// 别名映射
        /// </summary>
        public Dictionary<string, string> AvatarMapping => ChannelSource.AliasMapping;

        /// <summary>
        /// 别名映射
        /// </summary>
        public Dictionary<string, string> AliasMapping => ChannelSource.AliasMapping;
        
        /// <summary>
        /// 头衔映射
        /// </summary>
        public Dictionary<string, string> TitleMapping  => ChannelSource.TitleMapping;
        
        /// <summary>
        /// 身份映射
        /// </summary>
        public Dictionary<string, MemberRole> RoleMapping  => ChannelSource.RoleMapping;

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
        public ObservableCollection<CharacterUI> Members { get; }
        
        /// <summary>
        /// 所有消息
        /// </summary>
        public ObservableCollection<ChatMessageUI> Messages { get; }
    }
}