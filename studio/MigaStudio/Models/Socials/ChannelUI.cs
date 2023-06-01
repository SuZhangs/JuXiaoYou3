using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class ChannelUI : ObservableObject
    {
        private readonly IReadOnlyDictionary<string, SocialCharacter> _characterMapper;
        
        public ChannelUI(SocialChannel channel, Dictionary<string, SocialCharacter> characterMapper)
        {
            _characterMapper = characterMapper ?? throw new ArgumentNullException(nameof(characterMapper));
            ChannelSource    = channel ?? throw new ArgumentNullException(nameof(channel));
            Members          = new ObservableCollection<CharacterUI>();
            Members.AddMany(channel.Members
                                   .Select(Transform)
                                   .Where(x => x is not null));
            Messages = new ObservableCollection<ChatMessageUI>();
        }

        private CharacterUI Transform(ChannelMember x)
        {
            return _characterMapper.TryGetValue(x.MemberID, out var character) ? new CharacterUI(x, character) : null;
        }

        public string Id => ChannelSource.Id;
        
        public SocialChannel ChannelSource { get;  }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => ChannelSource.Intro;
            set
            {
                ChannelSource.Intro = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => ChannelSource.Avatar;
            set
            {
                ChannelSource.Avatar = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => ChannelSource.Name;
            set
            {
                ChannelSource.Name = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 所有成员
        /// </summary>
        public ObservableCollection<CharacterUI> Members { get; init; }
        
        /// <summary>
        /// 所有消息
        /// </summary>
        public ObservableCollection<ChatMessageUI> Messages { get; init; }
    }
}