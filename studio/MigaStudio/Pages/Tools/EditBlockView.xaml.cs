﻿using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.TemplateEditor
{
    [Connected(View = typeof(EditBlockView), ViewModel = typeof(EditBlockViewModel))]
    public partial class EditBlockView
    {
        public EditBlockView()
        {
            InitializeComponent();
        }
    }
}