using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Tools.MusicPlayer.ViewModels;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Acorisoft.FutureGL.Tools.MusicPlayer
{

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


        private void OnVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ViewModel.IsMute = ViewModel.Volume == 0;
        }

        public MusicPlayerViewModel ViewModel { get; }
    }
}