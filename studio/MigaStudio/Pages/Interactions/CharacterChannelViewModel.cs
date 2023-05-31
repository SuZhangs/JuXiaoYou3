using System.Collections.Generic;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public partial class CharacterChannelViewModel : InteractionViewModelBase
    {
        private string _channelName;
        
        public CharacterChannelViewModel()
        {
            Characters = new ObservableCollection<CharacterUI>();
            Messages   = new ObservableCollection<ChatMessageUI>();
        }

        private void Initialize()
        {
            Characters.AddMany(Channel.Members, true);
        }

        protected override void OnStart(Parameter parameter)
        {
            var p = parameter.Args;
            Channel     = p[0] as ChannelUI;

            if (Channel is null)
            {
                throw new ArgumentNullException(nameof(Channel));
            }
            
            //
            // 设置标题
            Title = 
                ChannelName =
                    Channel.Name;
            
            Initialize();
            base.OnStart(parameter);
        }

        /// <summary>
        /// 绑定的对象
        /// </summary>
        public ChannelUI Channel { get; private set; }
        
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