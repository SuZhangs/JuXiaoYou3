using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class MemberJoinEventUI : MessageUI
    {
        public MemberJoinEventUI(ChannelMessage msg)
        {
            MessageSource = msg ?? throw new ArgumentException(nameof(msg));
        }


        public ChannelMessage MessageSource { get; }


        /// <summary>
        /// 获取或设置 <see cref="MemberName"/> 属性。
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => MessageSource.Content;
            set
            {
                MessageSource.Content = value;
                RaiseUpdated();
            }
        }

        public override MessageBase Source => MessageSource;
    }
    
    public class MemberLeaveEventUI : MessageUI
    {
        public MemberLeaveEventUI(ChannelMessage msg)
        {
            MessageSource = msg ?? throw new ArgumentException(nameof(msg));
        }


        public ChannelMessage MessageSource { get; }


        /// <summary>
        /// 获取或设置 <see cref="MemberName"/> 属性。
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => MessageSource.Content;
            set
            {
                MessageSource.Content = value;
                RaiseUpdated();
            }
        }

        public override MessageBase Source => MessageSource;
    }
}