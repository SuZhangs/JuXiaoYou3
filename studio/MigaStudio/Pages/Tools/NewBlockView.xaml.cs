﻿using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
{
    [Connected(View = typeof(NewBlockView), ViewModel = typeof(NewBlockViewModel))]
    public partial class NewBlockView
    {
        public NewBlockView()
        {
            InitializeComponent();
        }
    }
}