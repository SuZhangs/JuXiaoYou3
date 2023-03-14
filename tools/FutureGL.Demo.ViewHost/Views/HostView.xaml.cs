﻿using System;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Demo.ViewModels;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(HostView), ViewModel = typeof(HostViewModel))]
    public partial class HostView : System.Windows.Controls.UserControl
    {
        public HostView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SubSystem.OptionSelection<FilteringOption>("过滤",
                (object) FilteringOption.Name, 
                new object[]
                {
                    FilteringOption.Name, FilteringOption.TimeOfCreated, FilteringOption.TimeOfModified
                });
        }
    }
}