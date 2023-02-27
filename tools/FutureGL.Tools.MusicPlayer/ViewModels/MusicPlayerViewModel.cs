using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.Tools.MusicPlayer.ViewModels
{
    public sealed class MusicPlayerViewModel : ForestObject
    {
        private bool        _isMute;
        private double      _volume;
        private double      _lastVolume;
        private ImageSource _cover;
        private ImageSource _background;
        private bool        _isPlaying;
        private Playlist    _playlist;
        private Music       _current;

        public MusicPlayerViewModel()
        {
            Background = new BitmapImage(new Uri("E:\\1.jpg"));
            Cover      = new BitmapImage(new Uri("E:\\1.jpg"));
            Playlist = new Playlist
            {
                Name  = "新建播放列表",
                Items = new ObservableCollection<Music>()
            };
            Volume = 0.5d;

            AddMusicToPlaylistCommand      = new RelayCommand(AddMusicToPlaylistImpl, HasPlaylist);
            RemoveMusicFromPlaylistCommand = new RelayCommand<Music>(RemoveMusicFromPlaylistImpl, HasMusicItem);
            MuteOrUnMuteCommand            = new RelayCommand(MuteOrUnmuteImpl);
        }

        private bool HasPlaylist() => Playlist is not null;
        private bool HasMusicItem(Music item) => item is not null && Playlist is not null;

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

        private void AddMusicToPlaylistImpl()
        {
            
        }

        private void RemoveMusicFromPlaylistImpl(Music item)
        {
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
                IsMute = value == 0;
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

                if (value)
                {
                    //
                }
                else
                {
                    //
                }
            }
        }

        public RelayCommand MuteOrUnMuteCommand { get; }
        public RelayCommand AddMusicToPlaylistCommand { get; }
        public RelayCommand<Music> RemoveMusicFromPlaylistCommand { get; }
    }
}