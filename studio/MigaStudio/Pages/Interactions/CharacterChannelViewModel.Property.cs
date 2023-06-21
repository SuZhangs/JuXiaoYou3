using System.Threading.Channels;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private string _message;
        private DocumentCache _speaker;

        public ObservableCollection<DocumentCache> LatestSpeakers { get; }
        public ObservableCollection<SocialMessageUI> Messages { get; }

        /// <summary>
        /// 获取或设置 <see cref="Speaker"/> 属性。
        /// </summary>
        public DocumentCache Speaker
        {
            get => _speaker;
            set => SetValue(ref _speaker, value);
        }
        /// <summary>
        /// 
        /// </summary>
        public SocialChannel Channel { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => Channel.Intro;
            set
            {
                Channel.Intro = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="PendingContent"/> 属性。
        /// </summary>
        public string PendingContent
        {
            get => Channel.PendingContent;
            set
            {
                Channel.PendingContent = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Message"/> 属性。
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetValue(ref _message, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ChannelName"/> 属性。
        /// </summary>
        public string ChannelName
        {
            get => Channel.Name;
            set
            {
                Channel.Name = value;
                RaiseUpdated();
            }
        }
    }
}