using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class CharacterUI : ObservableObject
    {
        private string     _name;
        private string     _speakerID;

        public CharacterUI(ChannelMember member, SocialCharacter character)
        {
            Member    = member ?? throw new ArgumentNullException(nameof(member));
            Character = character ?? throw new ArgumentNullException(nameof(character));
            Name      = Character.Name;
        }

        public ChannelMember   Member { get; }
        public SocialCharacter Character { get; }

        public string Id => Character.Id;

        /// <summary>
        /// 修改发言者ID，可以使得
        /// </summary>
        public string Speaker
        {
            get => _speakerID;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    
                    Name = Character.Name;
                    return;
                }
                
                _speakerID = value;
                
                if (Member.AliasMapping
                          .TryGetValue(value, out var name))
                {
                    Name = name;
                }
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Title"/> 属性。
        /// </summary>
        public string Title
        {
            get => Member.Title;
            set
            {
                Member.Title = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Role"/> 属性。
        /// </summary>
        public MemberRole Role
        {
            get => Member.Role;
            set
            {
                Member.Role = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => Character.Avatar;
            set
            {
                Character.Avatar = value;
                RaiseUpdated();
            }
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