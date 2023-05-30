using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public class InteractionStartupViewModel : TabViewModel
    {
        private CharacterUI _selectedCharacter;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ChannelUI> Channels { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<CharacterUI> Characters { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ThreadUI> Threads { get; }

        /// <summary>
        /// 获取或设置 <see cref="SelectedCharacter"/> 属性。
        /// </summary>
        public CharacterUI SelectedCharacter
        {
            get => _selectedCharacter;
            set => SetValue(ref _selectedCharacter, value);
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddCharacter { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand EditCharacter { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand RemoveCharacter { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddChannel { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand EditChannel { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand RemoveChannel { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddThread { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand EditThread { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand RemoveThread { get; }
    }
}