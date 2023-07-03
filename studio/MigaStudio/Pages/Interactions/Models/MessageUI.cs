using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public abstract class MessageUI : ObservableObject
    {
        private bool _isSelf;

        /// <summary>
        /// 获得消息源
        /// </summary>
        public abstract MessageBase Source { get; }
        
        /// <summary>
        /// 发言人ID
        /// </summary>
        public string MemberID { get; init; }

        /// <summary>
        /// 是否为本人发言。
        /// </summary>
        public bool IsSelf
        {
            get => _isSelf;
            set => SetValue(ref _isSelf, value);
        }
    }
}