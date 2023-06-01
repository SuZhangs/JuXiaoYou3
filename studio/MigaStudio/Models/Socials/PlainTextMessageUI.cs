using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Enums;
using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class PlainTextMessageUI : UserMessageUI
    {
        private string     _name;
        private string     _title;
        private string     _avatar;
        private MemberRole _role;

        public PlainTextMessageUI(SocialMessage message, string avatar, string title, string name, MemberRole role)
        {
            Source   = message;
            Avatar   = avatar;
            Title    = title;
            Name     = name;
            Role     = role;
            Content  = message.Content;
            MemberID = message.MemberID;
        }

        /// <summary>
        /// 
        /// </summary>
        public SocialMessage Source { get; }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Role"/> 属性。
        /// </summary>
        public MemberRole Role
        {
            get => _role;
            set => SetValue(ref _role, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => Source.Content;
            set
            {
                Source.Content = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Title"/> 属性。
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
}