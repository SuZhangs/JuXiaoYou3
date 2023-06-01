using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public abstract class ChatMessageUI : ObservableObject
    {
        private PreferSocialMessageLayout _style;

        /// <summary>
        /// 获取或设置 <see cref="Style"/> 属性。
        /// </summary>
        public PreferSocialMessageLayout Style
        {
            get => _style;
            set => SetValue(ref _style, value);
        }
    }
}