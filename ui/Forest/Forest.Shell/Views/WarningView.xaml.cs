﻿using System.Windows.Controls;

namespace Acorisoft.FutureGL.Forest.Views
{
    public partial class WarningView : UserControl
    {
        public WarningView()
        {
            InitializeComponent();
        }
    }
    
    public sealed class WarningViewModel : OperationViewModel
    {
        private string _content;

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
    }
}