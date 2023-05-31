using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public class InteractionStartupViewModel : TabViewModel
    {
        private readonly Dictionary<string, SocialCharacter> _characterMapper;

        private SocialCharacter _selectedCharacter;
        private bool            _hasCharacter;
        private bool            _hasChannel;
        private bool            _hasSelectedCharacter;
        private bool            _hasSelectedCharacterAndHasCharacter;
        private bool            _hasThread;

        public InteractionStartupViewModel()
        {
            _characterMapper = new Dictionary<string, SocialCharacter>(StringComparer.OrdinalIgnoreCase);
            
            DatabaseManager  = Studio.DatabaseManager();
            ImageEngine      = Studio.Engine<ImageEngine>();
            SocialEngine     = Studio.Engine<SocialEngine>();
            DocumentEngine   = Studio.Engine<DocumentEngine>();
            Channels         = new ObservableCollection<ChannelUI>();
            Characters       = new ObservableCollection<SocialCharacter>();
            Threads          = new ObservableCollection<ThreadUI>();
            

            AddCharacterCommand    = AsyncCommand(AddCharacterImpl);
            EditCharacterCommand   = AsyncCommand<SocialCharacter>(EditCharacterImpl);
            RemoveCharacterCommand = AsyncCommand<SocialCharacter>(RemoveCharacterImpl);
            SelectCharacterCommand = Command<SocialCharacter>(SelectCharacterImpl);

            AddThreadCommand    = AsyncCommand<SocialCharacter>(AddThreadImpl);
            EditThreadCommand   = AsyncCommand<ThreadUI>(EditThreadImpl);
            RemoveThreadCommand = AsyncCommand<ThreadUI>(RemoveThreadImpl);

            AddChannelCommand    = AsyncCommand<SocialCharacter>(AddChannelImpl);
            EditChannelCommand   = AsyncCommand<ChannelUI>(EditChannelImpl);
            RemoveChannelCommand = AsyncCommand<ChannelUI>(RemoveChannelImpl);
        }

        private void Synchronize()
        {
            _characterMapper.Clear();

            var iterator = SocialEngine.CharacterDB
                                       .FindAll();

            foreach (var character in iterator)
            {
                _characterMapper.TryAdd(character.Id, character);
                Characters.Add(character);
            }
        }

        private void UpdateContextualDataSource()
        {
            if (!HasCharacter)
            {
                return;
            }

            // Channels.AddMany(, true);
            // Threads.AddMany(, true);
        }

        private void UpdateState()
        {
            HasSelectedCharacter                = SelectedCharacter is not null;
            HasCharacter                        = Characters.Count > 0;
            HasChannel                          = Channels.Count > 0;
            HasThread                           = Threads.Count > 0;
            HasSelectedCharacterAndHasCharacter = HasSelectedCharacter && HasCharacter;
        }

        private async Task AddCharacterImpl()
        {
            var hash = Characters.Select(x => x.Id)
                                 .ToHashSet();

            var r = await SubSystem.MultiSelectExclude(DocumentType.Character, hash);
            
            if (!r.IsFinished)
            {
                return;
            }

            var values = r.Value
                          .Select(x => new SocialCharacter
                          {
                              Id = x.Id,
                              Name = x.Name,
                              Avatar = x.Avatar,
                              Intro = x.Intro
                          });

            foreach (var character in values)
            {
                
                Characters.Add(character);
                SocialEngine.AddCharacter(character);
            }
            
            UpdateState();
        }

        private async Task EditCharacterImpl(SocialCharacter item)
        {
        }


        private void SelectCharacterImpl(SocialCharacter item)
        {
            if (item is null)
            {
                return;
            }
            var db = Studio.Database();
            var pp = db.Get<PresetProperty>();
            pp.LastSocialCharacterID = item.Id;
            SelectedCharacter        = item;
            db.Update(pp);
        }

        private async Task RemoveCharacterImpl(SocialCharacter item)
        {
            if (item is null)
            {
                return;
            }

            if (!await  this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Characters.Remove(item);
            SocialEngine.RemoveCharacter(item);
        }

        private async Task AddChannelImpl(SocialCharacter item)
        {
        }

        private async Task EditChannelImpl(ChannelUI item)
        {
        }

        private async Task RemoveChannelImpl(ChannelUI item)
        {
        }

        private async Task AddThreadImpl(SocialCharacter item)
        {
        }

        private async Task EditThreadImpl(ThreadUI item)
        {
        }

        private async Task RemoveThreadImpl(ThreadUI item)
        {
        }

        protected override void OnStart()
        {
            //
            // 加载数据
            Synchronize();
            
            //
            // 选择最后一个打开的设定
            var db = Studio.Database();
            var pp = db.Get<PresetProperty>();
            var id = pp.LastSocialCharacterID;

            if (_characterMapper.TryGetValue(id, out var character))
            {
                SelectedCharacter = character;
            }
            else
            {
                pp.LastSocialCharacterID = null;
                db.Update(pp);
            }
            
            UpdateState();
            
            base.OnStart();
        }

        protected override void OnResume()
        {
            // 需要同步
            base.OnResume();
        }

        /// <summary>
        /// 获取或设置 <see cref="HasSelectedCharacterAndHasCharacter"/> 属性。
        /// </summary>
        public bool HasSelectedCharacterAndHasCharacter
        {
            get => _hasSelectedCharacterAndHasCharacter;
            set => SetValue(ref _hasSelectedCharacterAndHasCharacter, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HasSelectedCharacter"/> 属性。
        /// </summary>
        public bool HasSelectedCharacter
        {
            get => _hasSelectedCharacter;
            set => SetValue(ref _hasSelectedCharacter, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HasChannel"/> 属性。
        /// </summary>
        public bool HasChannel
        {
            get => _hasChannel;
            set => SetValue(ref _hasChannel, value);
        }


        /// <summary>
        /// 获取或设置 <see cref="HasThread"/> 属性。
        /// </summary>
        public bool HasThread
        {
            get => _hasThread;
            set => SetValue(ref _hasThread, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HasCharacter"/> 属性。
        /// </summary>
        public bool HasCharacter
        {
            get => _hasCharacter;
            set => SetValue(ref _hasCharacter, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ChannelUI> Channels { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SocialCharacter> Characters { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ThreadUI> Threads { get; }

        /// <summary>
        /// 获取或设置 <see cref="SelectedCharacter"/> 属性。
        /// </summary>
        public SocialCharacter SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                SetValue(ref _selectedCharacter, value);
                UpdateContextualDataSource();
                UpdateState();
            }
        }

        public DocumentEngine DocumentEngine { get; }
        public ImageEngine ImageEngine { get; }
        public SocialEngine SocialEngine { get; }
        public IDatabaseManager DatabaseManager { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<SocialCharacter> SelectCharacterCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddCharacterCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SocialCharacter> EditCharacterCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SocialCharacter> RemoveCharacterCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SocialCharacter> AddChannelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ChannelUI> EditChannelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ChannelUI> RemoveChannelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SocialCharacter> AddThreadCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ThreadUI> EditThreadCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ThreadUI> RemoveThreadCommand { get; }
    }
}