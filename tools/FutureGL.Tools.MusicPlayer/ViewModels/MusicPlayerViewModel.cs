using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Tools.MusicPlayer.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Acorisoft.FutureGL.Forest.Styles.Animations;
using NAudio.Wave;

namespace Acorisoft.FutureGL.Tools.MusicPlayer.ViewModels
{
    public sealed class MusicPlayerViewModel : ForestObject
    {
        private readonly MusicService    _service;
        private readonly HashSet<string> _hash;
        private readonly IScheduler      _scheduler;

        private bool        _isMute;
        private double      _volume;
        private double      _lastVolume;
        private ImageSource _cover;
        private ImageSource _background;
        private bool        _isPlaying;
        private Playlist    _playlist;
        private Music       _current;
        private PlayMode    _mode;


        public MusicPlayerViewModel()
        {
            _hash      = new HashSet<string>();
            _scheduler = new SynchronizationContextScheduler(SynchronizationContext.Current!);
            _service   = new MusicService();
            _service.StateUpdatedHandler = HandleStateChanged;
            _service.Position
                .ObserveOn(_scheduler)
                .Subscribe(x =>
                {
                    Position  = x;
                });
            
            _service.Duration
                .ObserveOn(_scheduler)
                .Subscribe(x =>
                {
                    Duration = x;
                });

            Background = new BitmapImage(new Uri("E:\\1.jpg"));
            Cover      = Background;
            Playlist = new Playlist
            {
                Name  = "新建播放列表",
                Items = new ObservableCollection<Music>()
            };
            Volume = 0.5d;

            AddMusicToPlaylistCommand      = new RelayCommand(AddMusicToPlaylistImpl, HasPlaylist);
            RemoveMusicFromPlaylistCommand = new RelayCommand<Music>(RemoveMusicFromPlaylistImpl, HasMusicItem);
            PlayMusicCommand               = new RelayCommand<Music>(PlayMusicImpl, HasMusicItem);
            PlayOrPauseCommand             = new RelayCommand(PlayOrPauseImpl, HasPlaylist);
            MuteOrUnMuteCommand            = new RelayCommand(MuteOrUnmuteImpl);
            ChangePlayModeCommand          = new RelayCommand(ChangePlayModeImpl);
        }

        private void HandleStateChanged(TimeSpan duration, PlaybackState state, Music item)
        {
            Duration  = duration;
            IsPlaying = state == PlaybackState.Playing;
            Current   = item;
            Position  = TimeSpan.Zero;
        }

        private bool HasPlaylist() => Playlist is not null;
        
        private bool HasMusicItem(Music item) => item is not null && Playlist is not null;

        private void ChangePlayModeImpl()
        {
            Mode = Mode switch
            {
                PlayMode.Loop => PlayMode.Repeat,
                PlayMode.Repeat => PlayMode.Shuffle,
                PlayMode.Shuffle => PlayMode.Sequence,
                _=> PlayMode.Sequence
            };
        }

        private void MuteOrUnmuteImpl()
        {
            if (Volume == 0)
            {
                Volume = _lastVolume;
            }
            else
            {
                {
                    _lastVolume = Volume;
                    Volume      = 0d;
                }
            }
        }
        
        private void PlayOrPauseImpl()
        {
            if (IsPlaying)
            {
                _service.Pause();
                IsPlaying = false;
            }
            else
            {
                _service.Play();
                IsPlaying = true;
            }
        }

        private void AddMusicToPlaylistImpl()
        {
            var opendlg = new OpenFileDialog
            {
                Filter = "音乐文件|*.wav;*.mp3",
            };

            if (opendlg.ShowDialog() != true)
            {
                return;
            }

            if (!_hash.Add(opendlg.FileName))
            {
                // TODO: duplicated
                return;
            }

            var file = TagLib.File.Create(opendlg.FileName);
            var musicFile = (TagLib.Mpeg.AudioFile)file;
            var tag = musicFile.GetTag(TagLib.TagTypes.Id3v2);
            var cover = string.Empty;

            if (tag.Pictures is not null)
            {
                var pic = tag.Pictures.First();
                cover = Path.Combine(Path.GetDirectoryName(opendlg.FileName)!, Path.GetFileNameWithoutExtension(opendlg.FileName) + ".png");

                if (!File.Exists(cover))
                {
                    File.WriteAllBytes(cover, pic.Data.Data);
                }
            }

            var music = new Music
            {
                Id     = opendlg.FileName,
                Path   = opendlg.FileName,
                Name   = tag.Title,
                Author = tag.FirstPerformer,
                Cover  = cover
            };

            //
            //
            Playlist.Items.Add(music);
        }

        private void RemoveMusicFromPlaylistImpl(Music item)
        {
        }

        private void PlayMusicImpl(Music item)
        {
            if (_service.Playlist.CurrentValue is null)
            {
                _service.SetPlaylist(Playlist);
            }

            if (_service.Music.CurrentValue is not null &&
                _service.Music.CurrentValue.Id == item.Id)
            {
                _service.Pause();
            }
            else
            {
                Play(item);
            }
        }

        void Play(Music item)
        {
            Current   = item;
            IsPlaying = true;

            //
            _service.Play(item);

            //
            //
            if (!string.IsNullOrEmpty(item.Cover))
            {
                Cover      = new BitmapImage(new Uri(item.Cover));
                Background = Cover;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Current"/> 属性。
        /// </summary>
        public Music Current
        {
            get => _current;
            set => SetValue(ref _current, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Playlist"/> 属性。
        /// </summary>
        public Playlist Playlist
        {
            get => _playlist;
            set => SetValue(ref _playlist, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Cover"/> 属性。
        /// </summary>
        public ImageSource Cover
        {
            get => _cover;
            set => SetValue(ref _cover, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Volume"/> 属性。
        /// </summary>
        public double Volume
        {
            get => _volume;
            set
            {
                SetValue(ref _volume, value);
                IsMute          = value == 0;
                _service.Volume = (float)value;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="IsMute"/> 属性。
        /// </summary>
        public bool IsMute
        {
            get => _isMute;
            set => SetValue(ref _isMute, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Background"/> 属性。
        /// </summary>
        public ImageSource Background
        {
            get => _background;
            set => SetValue(ref _background, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsPlaying"/> 属性。
        /// </summary>
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                SetValue(ref _isPlaying, value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Mode"/> 属性。
        /// </summary>
        public PlayMode Mode
        {
            get => _mode;
            set
            {
                SetValue(ref _mode, value);
                _service.Mode = value;
            }
        }

        private TimeSpan _position;
        private TimeSpan _duration;

        /// <summary>
        /// 获取或设置 <see cref="Duration"/> 属性。
        /// </summary>
        public TimeSpan Duration
        {
            get => _duration;
            set => SetValue(ref _duration, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Position"/> 属性。
        /// </summary>
        public TimeSpan Position
        {
            get => _position;
            set => SetValue(ref _position, value);
        }

        public RelayCommand MuteOrUnMuteCommand { get; }
        public RelayCommand AddMusicToPlaylistCommand { get; }
        public RelayCommand ChangePlayModeCommand { get; }
        public RelayCommand PlayOrPauseCommand { get; }
        public RelayCommand<Music> PlayMusicCommand { get; }
        public RelayCommand<Music> RemoveMusicFromPlaylistCommand { get; }
    }
}