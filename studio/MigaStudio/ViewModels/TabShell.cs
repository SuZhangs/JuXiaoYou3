using System;
using System.Collections.ObjectModel;
using System.Windows;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class TabShell : TabController
    {
        private WindowState _windowState;

        /// <summary>
        /// 获取或设置 <see cref="WindowState"/> 属性。
        /// </summary>
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                SetValue(ref _windowState, value);
                WindowStateHandler?.Invoke(value);
            }
        }

        public Action<WindowState> WindowStateHandler { get; set; }
    }
}