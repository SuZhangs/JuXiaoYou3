﻿using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Editors;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    [Connected(View = typeof(ComposeEditorPage), ViewModel = typeof(ComposeEditorViewModel))]
    public partial class ComposeEditorPage
    {
        public ComposeEditorPage()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            //
            // 
            ViewModel.Register(Editor)
                     .Register(RtfEditor)
                     .Initialize();
            
            base.OnLoaded(sender, e);
        }

        protected ComposeEditorViewModel ViewModel => ViewModel<ComposeEditorViewModel>();
    }
}