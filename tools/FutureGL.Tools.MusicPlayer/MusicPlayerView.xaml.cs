using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Acorisoft.FutureGL.Tools.MusicPlayer
{
    public sealed class MusicPlayerViewModel : ForestObject
    {
        private bool        _isMute;
        private double      _volume;
        private ImageSource _cover;
        private bool      _isPlaying;
        
        public MusicPlayerViewModel()
        {
            Cover = new BitmapImage(new Uri("E:\\1.jpg"));
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
            set => SetValue(ref _volume, value);
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
    }

    public sealed class MuteOrUnmuteIconConverter : IValueConverter
    {
        private Geometry _muted;
        private Geometry _unmuted;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value != null && (bool)value;
            if (val)
            {
                _muted ??= Xaml.GetGeometry("Mute");
                return _muted;
            }
            else
            {
                _unmuted ??= Xaml.GetGeometry("Volume");
                return _unmuted;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class PlayOrPauseIconConverter : IValueConverter
    {
        private Geometry _off;
        private Geometry _on;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value != null && (bool)value;
            if (val)
            {
                _off ??= Xaml.GetGeometry("Pause");
                return _off;
            }
            else
            {
                _on ??= Xaml.GetGeometry("Play");
                return _on;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class MusicPlayerView : UserControl
    {
        public MusicPlayerView()
        {
            InitializeComponent();
            ViewModel   = new MusicPlayerViewModel();
            DataContext = ViewModel;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            ViewModel.Volume = 0.5d;
        }

        private void Button_OpenVolume(object sender, RoutedEventArgs e)
        {
            MusicBox.IsTopOpen = true;
        }

        private void Button_ClosePlaylist(object sender, RoutedEventArgs e)
        {
            MusicBox.IsRightOpen = false;
        }


        private void Button_OpenPlaylist(object sender, RoutedEventArgs e)
        {
            MusicBox.IsRightOpen = true;
        }

        private void Button_MuteOrUnmute(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Volume == 0)
            {
                ViewModel.Volume = _lastVolume;
            }
            else
            {
                {
                    _lastVolume      = ViewModel.Volume;
                    ViewModel.Volume = 0d;
                }
            }
        }

        private void OnVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ViewModel.IsMute = ViewModel.Volume == 0;
        }

        private double _lastVolume = 0.5d;

        public MusicPlayerViewModel ViewModel { get; }
    }
}