using System.Threading.Channels;
using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
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