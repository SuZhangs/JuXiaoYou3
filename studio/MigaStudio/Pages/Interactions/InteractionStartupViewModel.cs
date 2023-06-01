using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Models.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public class InteractionStartupViewModel : InteractionViewModelBase
    {
        private SocialCharacter _selectedCharacter;
        private bool            _hasCharacter;
        private bool            _hasChannel;
        private bool            _hasSelectedCharacter;
        private bool            _hasSelectedCharacterAndHasCharacter;
        private bool            _hasThread;

        public InteractionStartupViewModel()
        {
            DatabaseManager = Studio.DatabaseManager();
            ImageEngine     = Studio.Engine<ImageEngine>();
            SocialEngine    = Studio.Engine<SocialEngine>();
            DocumentEngine  = Studio.Engine<DocumentEngine>();
            Channels        = new ObservableCollection<ChannelUI>();
            Characters      = new ObservableCollection<SocialCharacter>();
            Threads         = new ObservableCollection<ThreadUI>();


            AddCharacterCommand    = AsyncCommand(AddCharacterImpl);
            EditCharacterCommand   = AsyncCommand<SocialCharacter>(EditCharacterImpl);
            RemoveCharacterCommand = AsyncCommand<SocialCharacter>(RemoveCharacterImpl);
            SelectCharacterCommand = Command<SocialCharacter>(SelectCharacterImpl);

            AddThreadCommand    = AsyncCommand<SocialCharacter>(AddThreadImpl);
            EditThreadCommand   = AsyncCommand<ThreadUI>(EditThreadImpl);
            RemoveThreadCommand = AsyncCommand<ThreadUI>(RemoveThreadImpl);

            AddChannelCommand    = AsyncCommand<SocialCharacter>(AddChannelImpl);
            EditChannelCommand   = Command<ChannelUI>(EditChannelImpl);
            RemoveChannelCommand = AsyncCommand<ChannelUI>(RemoveChannelImpl);
        }

        private void Synchronize()
        {
            CharacterMapper.Clear();

            var iterator = SocialEngine.CharacterDB
                                       .FindAll();

            foreach (var character in iterator)
            {
                CharacterMapper.TryAdd(character.Id, character);
                Characters.Add(character);
            }
        }

        private void UpdateContextualDataSource()
        {
            if (!HasCharacter)
            {
                return;
            }

            var channels = SocialEngine.GetChannels(SelectedCharacter.Id);
            Channels.AddMany(channels.Select(x => new ChannelUI(x, CharacterMapper)), true);
            // Threads.AddMany(, true);
        }

        private void UpdateState()
        {
            HasSelectedCharacter                = SelectedCharacter is not null;
            HasCharacter                        = Characters.Count > 0;
            HasChannel                          = Channels.Count > 0;
            HasThread                           = Threads.Count > 0;
            HasSelectedCharacterAndHasCharacter = HasSelectedCharacter || HasCharacter;
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
                              Id     = x.Id,
                              Name   = x.Name,
                              Avatar = x.Avatar,
                              Intro  = x.Intro
                          });

            foreach (var character in values)
            {
                Characters.Add(character);
                SocialEngine.AddCharacter(character);
            }

            SelectedCharacter = Characters.FirstOrDefault();
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

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            if (SelectedCharacter is not null &&
                SelectedCharacter.Id == item.Id)
            {
                SelectedCharacter = null;
            }

            Characters.Remove(item);
            SocialEngine.RemoveCharacter(item);
            UpdateState();
        }

        private async Task AddChannelImpl(SocialCharacter item)
        {
            if (item is null)
            {
                return;
            }

            if (SelectedCharacter is null)
            {
                return;
            }

            if (item.Id != SelectedCharacter.Id)
            {
                return;
            }

            var r = await SingleLineViewModel.String("创建群聊");

            if (!r.IsFinished)
            {
                return;
            }

            var r1 = await SubSystem.MultiSelectExclude(DocumentType.Character, new HashSet<string>
            {
                SelectedCharacter.Id
            });

            if (!r1.IsFinished)
            {
                return;
            }

            var channel = new SocialChannel
            {
                Id       = ID.Get(),
                Name     = r.Value,
                Members  = new ObservableCollection<ChannelMember>(),
                Messages = new ObservableCollection<SocialMessage>(),
            };

            channel.Members
                   .Add(new ChannelMember
                   {
                       MemberID     = SelectedCharacter.Id,
                       AliasMapping = new Dictionary<string, string>(),
                       Name         = SelectedCharacter.Name,
                       Title        = GetOwnerName(),
                       Role         = MemberRole.Owner
                   });

            var members = r1.Value
                            .Select(x => new ChannelMember
                            {
                                MemberID     = x.Id,
                                Name         = x.Name,
                                AliasMapping = new Dictionary<string, string>(),
                                Role         = MemberRole.Member,
                            });

            channel.Members.AddMany(members);
            //
            // 添加
            SocialEngine.AddChannel(channel);

            //
            // 添加
            Channels.Add(new ChannelUI(channel, CharacterMapper));
        }

        private void EditChannelImpl(ChannelUI item)
        {
            if (item is null)
            {
                return;
            }

            Controller.Start<CharacterChannelViewModel>(item.Id,new Parameter
            {
                Args = new[]
                {
                    item
                }
            });
        }

        private async Task RemoveChannelImpl(ChannelUI item)
        {
            if (item is null)
            {
                return;
            }

            if (!await this.Error(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            Channels.Remove(item);
            SocialEngine.RemoveChannel(item.ChannelSource);
            UpdateState();
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

            if (!string.IsNullOrEmpty(id) &&
                CharacterMapper.TryGetValue(id, out var character))
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
            UpdateState();
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

                if (value is null)
                {
                    Threads.Clear();
                    Channels.Clear();
                }
                else
                {
                    UpdateState();
                    UpdateContextualDataSource();
                }
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
        public RelayCommand<ChannelUI> EditChannelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ChannelUI> RemoveChannelCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<SocialCharacter> AddThreadCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ThreadUI> EditThreadCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<ThreadUI> RemoveThreadCommand { get; }

        public override bool Removable => false;
    }
}