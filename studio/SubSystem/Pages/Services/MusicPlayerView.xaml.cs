﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.MigaStudio.Resources.Converters;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Acorisoft.FutureGL.MigaStudio.Pages.Services
{

    public partial class MusicPlayerView:ForestUserControl 
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

        private bool _isDragging;

        private void Slider_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var slider = (Slider)sender;

            if (ViewModel.Current is null)
            {
                return;
            }

            _isDragging = true;
            
            //
            // 清除绑定
            slider.ClearValue(RangeBase.ValueProperty);
            slider.ValueChanged += Slider_OnValueChanged;
        }

        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = (Slider)sender;

            var time = TimeSpan.FromSeconds(slider.Value);
            ViewModel.SetPosition(time);
        }

        private void Slider_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var slider = (Slider)sender;

            if (ViewModel.Current is null)
            {
                return;
            }
            
            if (_isDragging)
            {
                _isDragging = false;
                
                //
                // 恢复绑定
                slider.ValueChanged -= Slider_OnValueChanged;
                slider.SetBinding(RangeBase.ValueProperty, new Binding
                {
                    Source    = ViewModel,
                    Converter = (TimeSpanConverter)FindResource("TimeSpanConverter"),
                    Path      = new PropertyPath("Position"),
                    Mode      = BindingMode.OneWay
                });
            }
        }
    }
}