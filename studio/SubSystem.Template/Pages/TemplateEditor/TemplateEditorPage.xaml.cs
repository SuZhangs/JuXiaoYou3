﻿using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
{

    [Connected(View = typeof(TemplateEditorPage), ViewModel = typeof(TemplateEditorViewModel))]
    public partial class TemplateEditorPage
    {
        public TemplateEditorPage()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel<TemplateEditorViewModel>().Canvas = Canvas;
            base.OnLoaded(sender, e);
        }
    }
}