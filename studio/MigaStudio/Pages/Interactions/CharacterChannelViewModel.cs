using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public partial class CharacterChannelViewModel : InteractionViewModelBase
    {
        private          string                          _channelName;
        private          CharacterUI                     _speaker;
        private          string                          _pendingContent;
        private readonly Dictionary<string, CharacterUI> _characterUIMapper;
        private          string                          _messageContent;
        
        public CharacterChannelViewModel()
        {
            _characterUIMapper         = new Dictionary<string, CharacterUI>();
            LatestSpeakers             = new ObservableCollection<CharacterUI>();
            Characters                 = new ObservableCollection<CharacterUI>();
            SwitchCommand              = Command<CharacterUI>(SwitchImpl);
            MemberJoinCommand          = AsyncCommand(MemberJoinImpl);
            MemberLeaveCommand         = AsyncCommand(MemberLeaveImpl);
            AddPlainTextMessageCommand = Command<CharacterUI>(AddPlainTextMessageImpl);
        }

        private void Initialize()
        {
            _characterUIMapper.Clear();
            OwnerID = Characters.First(x => GetCharacterRole(x.Id, Channel.RoleMapping) == MemberRole.Owner)
                                .Id;
            Characters.Clear();
            
            foreach (var member in Channel.Members)
            {
                _characterUIMapper.Add(member.Id, member);
                Characters.Add(member);
            }

            //
            // 添加消息
            foreach (var message in Channel.ChannelSource
                                           .Messages)
            {
                AddMessageFromInitialize(message);
            }

            Speaker = Characters.FirstOrDefault();
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
        /// 未发送的内容
        /// </summary>
        public string PendingContent
        {
            get => _pendingContent;
            set => SetValue(ref _pendingContent, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="MessageContent"/> 属性。
        /// </summary>
        public string MessageContent
        {
            get => _messageContent;
            set => SetValue(ref _messageContent, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Speaker"/> 属性。
        /// </summary>
        public CharacterUI Speaker
        {
            get => _speaker;
            set
            {
                SetValue(ref _speaker, value);

                if (_speaker is null)
                {
                    return;
                }
                
                //
                // 添加最近的发言人
                AddLatestSpeaker(_speaker);
                Messages.OfType<UserMessageUI>()
                        .ForEach(x =>
                        {
                            x.IsSelf = x.MemberID == value.Id;
                        });
            }
        }

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
        
        public string OwnerID { get; private set; }
        
        public ObservableCollection<CharacterUI> LatestSpeakers { get; }
        public ObservableCollection<CharacterUI> Characters { get; }
        public ObservableCollection<ChatMessageUI> Messages => Channel.Messages;
    }
}