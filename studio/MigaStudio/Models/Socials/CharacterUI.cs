using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models.Socials
{
    public class CharacterUI : ObservableObject
    {
        private string _name;
        private string _title;

        public CharacterUI(string title, string name, SocialCharacter character)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
            Name      = name;
            Title     = title;
        }

        public SocialCharacter Character { get; }

        public string Id => Character.Id;

        /// <summary>
        /// 获取或设置 <see cref="Title"/> 属性。
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
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