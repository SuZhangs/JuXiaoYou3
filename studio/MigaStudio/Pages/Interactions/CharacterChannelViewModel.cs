using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public partial class CharacterChannelViewModel : InteractionViewModelBase
    {
        private string _channelName;
        
        public CharacterChannelViewModel()
        {
            Messages = new ObservableCollection<ChatMessageUI>
            {
                new PlainTextMessageUI
                {
                    
                    Avatar = "D:\\2.png",
                    Name = "Sender",
                    Role = MemberRole.Owner,
                    Title = "群主",
                    Content = typeof(CharacterChannelViewModel).FullName,
                    IsSelf = false,
                    Style = PreferSocialMessageLayout.QQ,
                },
                new PlainTextMessageUI
                {
                    
                    Avatar  = "D:\\2.png",
                    Name    = "Character 1",
                    Role    = MemberRole.Self,
                    Content = "rashiua",
                    IsSelf  = true,
                    Style   = PreferSocialMessageLayout.QQ,
                },   new PlainTextMessageUI
                {
                    
                    Avatar  = "D:\\2.png",
                    Name    = "Character 2",
                    Role    = MemberRole.Manager,
                    Content = "false",
                    Title   = "管理",
                    IsSelf  = false,
                    Style   = PreferSocialMessageLayout.QQ,
                },
            };
        }

        /// <summary>
        /// 绑定的对象
        /// </summary>
        public SocialChannel Channel { get; private set; }
        
        /// <summary>
        /// 获取或设置 <see cref="ChannelName"/> 属性。
        /// </summary>
        public string ChannelName
        {
            get => _channelName;
            set
            {
                SetValue(ref _channelName, value);

                if (Channel is null)
                {
                    return;
                }

                Channel.Name = value;
            }
        }

        public ObservableCollection<CharacterUI> Characters { get; }
        public ObservableCollection<ChatMessageUI> Messages { get; }
    }
}