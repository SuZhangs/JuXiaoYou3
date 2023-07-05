using System.IO;
using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public class ImageUI : MessageUI
    {
        private readonly Func<string, string> _memberNameFinder;

        public ImageUI(ChannelMessage msg, Func<string, string> memberNameFinder)
        {
            if (msg is null ||
                msg.Type != MessageType.PlainText)
            {
                throw new ArgumentException(nameof(msg));
            }
            
            MessageSource     = msg;
            _memberNameFinder = memberNameFinder ?? throw new ArgumentException(nameof(memberNameFinder));
        }
        
        public void Update()
        {
            MemberName = _memberNameFinder(MessageSource.MemberID);
            RaiseUpdated(nameof(MemberName));
        }


        public ChannelMessage MessageSource { get; }


        /// <summary>
        /// 获取或设置 <see cref="MemberName"/> 属性。
        /// </summary>
        public string MemberName { get; set; }

        public override MessageBase Source => MessageSource;

        public string ImageSource => MessageSource.Source;
        public bool IsExists => File.Exists(MessageSource.Source);
    }
    
    
    public class EmojiUI : MessageUI
    {
        private readonly Func<string, string> _memberNameFinder;

        public EmojiUI(ChannelMessage msg, Func<string, string> memberNameFinder)
        {
            if (msg is null ||
                msg.Type != MessageType.PlainText)
            {
                throw new ArgumentException(nameof(msg));
            }
            
            MessageSource     = msg;
            _memberNameFinder = memberNameFinder ?? throw new ArgumentException(nameof(memberNameFinder));
        }
        
        public void Update()
        {
            MemberName = _memberNameFinder(MessageSource.MemberID);
            RaiseUpdated(nameof(MemberName));
        }


        public ChannelMessage MessageSource { get; }


        /// <summary>
        /// 获取或设置 <see cref="MemberName"/> 属性。
        /// </summary>
        public string MemberName { get; set; }

        public override MessageBase Source => MessageSource;

        public string ImageSource => MessageSource.Source;
        public bool IsExists => File.Exists(MessageSource.Source);
    }
}