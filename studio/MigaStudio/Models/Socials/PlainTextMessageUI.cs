using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Enums;
using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class PlainTextMessageUI : ChatMessageUI
    {
        private bool                      _isSelf;
        private string                    _name;
        private string                    _title;
        private string                    _avatar;

        public PlainTextMessageUI(PlainTextMessage message, SocialCharacter character, ChannelUI channel)
        {
            Source  = message;
            Avatar  = character.Avatar;
            Title   = InteractionViewModelBase.GetCharacterTitle(Source.MemberID, channel.RoleMapping, channel.TitleMapping);
            Name    = InteractionViewModelBase.GetCharacterName(character, channel.AliasMapping);
            Role    = channel.RoleMapping.TryGetValue(Source.MemberID, out var r) ? r : MemberRole.Member;
            Content = message.Content;
        }

        /// <summary>
        /// 
        /// </summary>
        public PlainTextMessage Source { get; }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }

        private MemberRole _role;

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

        /// <summary>
        /// 获取或设置 <see cref="IsSelf"/> 属性。
        /// </summary>
        public bool IsSelf
        {
            get => _isSelf;
            set => SetValue(ref _isSelf, value);
        }
    }
}