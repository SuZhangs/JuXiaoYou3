using System;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using Acorisoft.FutureGL.MigaUtils;
using NAudio.Wave;

namespace Acorisoft.FutureGL.Tools.MusicPlayer.Services
{
    public class MusicService : Disposable
    {
        private readonly WaveOutEvent    _device;
        private          AudioFileReader _reader;
        private          bool            _manualStop;
        private          int             _currentIndex;

        private readonly ObservableProperty<double>   _positionStream;
        private readonly ObservableProperty<double>   _volumeStream;
        private readonly ObservableProperty<Music>    _targetStream;
        private readonly ObservableProperty<Playlist> _playlistStream;
        private readonly ObservableProperty<PlayMode> _playModeSteam;

        public MusicService()
        {
            _positionStream         =  new ObservableProperty<double>(0d);
            _volumeStream           =  new ObservableProperty<double>(0.5d);
            _targetStream           =  new ObservableProperty<Music>(null);
            _playlistStream         =  new ObservableProperty<Playlist>(null);
            _playModeSteam          =  new ObservableProperty<PlayMode>(PlayMode.Sequence);
            _device                 =  new WaveOutEvent();
            _device.PlaybackStopped += OnDevicePlayStopped;
        }

        private void OnDevicePlayStopped(object sender, StoppedEventArgs e)
        {
            //
            // 检测手动停止
            if (_manualStop)
            {
                _manualStop = false;
                return;
            }

            //
            // 播放下一个
            PlayNext();
        }
        
        
        public void PlayLast()
        {
            var maxIndex = _playlistStream.CurrentValue.Items.Count - 1;
            Music music;
            
            if (_playModeSteam.CurrentValue == PlayMode.Sequence &&
                _currentIndex == maxIndex)
            {
                return;
            }

            if (_playModeSteam.CurrentValue == PlayMode.Shuffle)
            {
                _currentIndex = Random.Shared.Next(0, maxIndex);
                music         = _playlistStream.CurrentValue.Items[_currentIndex];
                _targetStream.SetValue(music);
            }
            else
            {

                _currentIndex = --_currentIndex % maxIndex;
                music         = _playlistStream.CurrentValue.Items[_currentIndex];
                _targetStream.SetValue(music);
            }

            //
            //
            Play(music);
        }

        public void PlayNext()
        {
            var maxIndex = _playlistStream.CurrentValue.Items.Count - 1;
            Music music;
            
            if (_playModeSteam.CurrentValue == PlayMode.Sequence &&
                _currentIndex == maxIndex)
            {
                return;
            }

            if (_playModeSteam.CurrentValue == PlayMode.Shuffle)
            {
                _currentIndex = Random.Shared.Next(0, maxIndex);
                music = _playlistStream.CurrentValue.Items[_currentIndex];
                _targetStream.SetValue(music);
            }
            else
            {

                _currentIndex = ++_currentIndex % maxIndex;
                music         = _playlistStream.CurrentValue.Items[_currentIndex];
                _targetStream.SetValue(music);
            }

            //
            //
            Play(music);
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="music"></param>
        public void Play(Music music)
        {
            if (music is null)
            {
                return;
            }

            //
            // 没有播放列表就创建
            if (_playlistStream.CurrentValue is null)
            {
                SetPlaylist(new Playlist
                {
                    Items = new ObservableCollection<Music>(new[] { music })
                }, true);
            }
            else
            {
                //
                // 停止播放
                Stop();

                _reader = new AudioFileReader(music.Path);
                _device.Init(_reader);
                _device.Play();
            }
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        public void Pause()
        {
            if (_device.PlaybackState == PlaybackState.Playing)
            {
                _device.Pause();
            }
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        public void Stop()
        {
            if (_device.PlaybackState != PlaybackState.Stopped)
            {
                _manualStop = true;
                _device.Stop();
            }
        }

        /// <summary>
        /// 设置播放列表
        /// </summary>
        /// <param name="playlist">播放列表</param>
        /// <param name="autoPlay">是否自动播放</param>
        public void SetPlaylist(Playlist playlist, bool autoPlay = false)
        {
            if (playlist is null)
            {
                return;
            }
            
            _playlistStream.SetValue(playlist);

            if (autoPlay)
            {
                var maxIndex = playlist.Items.Count - 1;
                Music music;
                
                if (_playModeSteam.CurrentValue == PlayMode.Shuffle)
                {
                    var index = Random.Shared.Next(0, maxIndex);
                    music = _playlistStream.CurrentValue.Items[index];

                    _currentIndex = index;
                    _targetStream.SetValue(music);
                }
                else
                {
                    music = _playlistStream.CurrentValue.Items[0];

                    _currentIndex = 0;
                    _targetStream.SetValue(music);
                }

                //
                //
                Play(music);
            }
        }

        protected override void ReleaseManagedResources()
        {
            _playlistStream.Dispose();
            _positionStream.Dispose();
            _playModeSteam.Dispose();
            _volumeStream.Dispose();
            _targetStream.Dispose();
            _device.Dispose();
            _reader?.Dispose();
        }


        public IObservable<double> Position => _positionStream.Observable;
        public IObservable<double> Volume => _volumeStream.Observable;
        public IObservable<Music> Music => _targetStream.Observable;
        public IObservable<Playlist> Playlist => _playlistStream.Observable;
        public IObservable<PlayMode> Mode => _playModeSteam.Observable;
    }
}