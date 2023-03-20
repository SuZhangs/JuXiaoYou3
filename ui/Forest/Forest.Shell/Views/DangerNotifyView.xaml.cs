﻿using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Forest.Views
{
    public partial class DangerNotifyView:ForestUserControl 
    {
        public DangerNotifyView()
        {
            InitializeComponent();
        }
    }
    
    public sealed class DangerNotifyViewModel : OperationViewModel
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