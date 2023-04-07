using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaStudio.Services;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;
using TagLib;
using TagLib.Mpeg;
using File = TagLib.File;
// ReSharper disable ConvertToUsingDeclaration

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class PlaylistPartViewModel : KeyValueViewModel
    {
        private readonly Subject<Music> _threadSafeAdding;

        public PlaylistPartViewModel()
        {
            Collection = new ObservableCollection<Music>();
            MusicEngine = Xaml.Get<IDatabaseManager>()
                              .GetEngine<MusicEngine>();
            _threadSafeAdding = new Subject<Music>().DisposeWith(Collector);
            _threadSafeAdding.ObserveOn(Scheduler)
                             .Subscribe(x =>
                             {
                                 Collection.Add(x);
                                 
                                 //
                                 //
                                 SelectedMusic ??= Collection.FirstOrDefault();
                                 Successful(SubSystemString.OperationOfAddIsSuccessful);
                                 Save();
                             })
                             .DisposeWith(Collector);
            AddMusicCommand       = AsyncCommand(AddMusicImpl);
            RemoveMusicCommand    = AsyncCommand<Music>(RemoveMusicImpl, HasItem);
            ShiftUpMusicCommand   = Command<Music>(ShiftUpMusicImpl, HasItem);
            ShiftDownMusicCommand = Command<Music>(ShiftDownMusicImpl, HasItem);
            PlayMusicCommand      = Command<Music>(PlayMusicImpl, HasItem);
        }

        public override void Start()
        {
            if (Detail.DataBags.ContainsKey(Data))
            {
                var payload = Detail.DataBags[Data];
                var list    = JSON.FromJson<ObservableCollection<Music>>(payload);
                Collection.AddRange(list);
            }

            base.Start();
        }

        private async Task AddMusicImpl()
        {
            var opendlg = new VistaOpenFileDialog
            {
                Filter      = "音乐文件|*.wav;*.mp3",
                Multiselect = true
            };

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            using (var session = Xaml.Get<IBusyService>()
                                     .CreateSession())
            {
                session.Update(SubSystemString.Processing);
                
                await Task.Run(async () =>
                {
                    await Task.Delay(300);
                    
                    await MusicUtilities.AddMusic(opendlg.FileNames, MusicEngine, x => _threadSafeAdding.OnNext(x));
                });
            }
            
        }


        private async Task RemoveMusicImpl(Music part)
        {
            if (part is null)
            {
                return;
            }

            if (!await DangerousOperation(SubSystemString.AreYouSureRemoveIt))
            {
                return;
            }

            var ms = Xaml.Get<MusicService>();
            if (ms.IsPlaying
                  .CurrentValue)
            {
                ms.Playlist
                  .CurrentValue
                  .Items
                  .Remove(part);
            }

            Collection.Remove(part);
            Save();
        }

        private void PlayMusicImpl(Music part)
        {
            if (part is null)
            {
                return;
            }
            var ms = Xaml.Get<MusicService>();

            if (ms.IsPlaying
                  .CurrentValue)
            {
                var hash = ms.Playlist
                             .CurrentValue
                             .Items
                             .Select(x => x.Id)
                             .ToHashSet();
                
                var noAddedValue = Collection.Where(x => !hash.Contains(x.Id));

                ms.Playlist
                  .CurrentValue
                  .Items
                  .AddRange(noAddedValue);
                
                ms.Play(part);
            }
            else
            {
                ms.SetPlaylist(new Playlist
                {
                    Name = EditorViewModel.Name,
                    Items = new ObservableCollection<Music>(Collection)
                }, true);
            }
        }

        private void ShiftDownMusicImpl(Music album)
        {
            Collection.ShiftDown(album);
            Save();
        }

        private void ShiftUpMusicImpl(Music album)
        {
            Collection.ShiftUp(album);
            Save();
        }

        private void Save()
        {
            var payload = JSON.Serialize(Collection);
            base[Data, Detail.DataBags] = payload;
            EditorViewModel.SetDirtyState();
        }

        private const string Data     = "_d";
        public const  string Option_1 = "_o";


        public bool AutoPlay
        {
            get => bool.TryParse(this[Option_1, Detail.DataBags], out var n) && n;
            set
            {
                this[Option_1, Detail.DataBags] = value.ToString();
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 编辑器
        /// </summary>
        public DocumentEditorBase EditorViewModel { get; init; }

        /// <summary>
        /// 文档
        /// </summary>
        public Document Document { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DocumentCache DocumentCache { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Music> Collection { get; init; }

        public MusicEngine MusicEngine { get; }
        private Music _selectedMusic;

        /// <summary>
        /// 获取或设置 <see cref="SelectedMusic"/> 属性。
        /// </summary>
        public Music SelectedMusic
        {
            get => _selectedMusic;
            set
            {
                SetValue(ref _selectedMusic, value);
                RemoveMusicCommand.NotifyCanExecuteChanged();
                ShiftDownMusicCommand.NotifyCanExecuteChanged();
                ShiftUpMusicCommand.NotifyCanExecuteChanged();
                PlayMusicCommand.NotifyCanExecuteChanged();
            }
        }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand AddMusicCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Music> ShiftUpMusicCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Music> ShiftDownMusicCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public RelayCommand<Music> PlayMusicCommand { get; }

        [NullCheck(UniTestLifetime.Constructor)]
        public AsyncRelayCommand<Music> RemoveMusicCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public PartOfPlaylist Detail { get; init; }
    }
}