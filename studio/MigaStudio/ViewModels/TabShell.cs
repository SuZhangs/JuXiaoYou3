using System;
using System.Collections.ObjectModel;
using System.Windows;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Lobby;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class TabShell : TabController
    {
        private WindowState _windowState;

        public TabShell()
        {
            
            AddTabCommand = Command(() =>
            {
                Onboards.Add(Test());
            });
        }
        
        
        private ITabViewModel Test()
        {
            var home = new HomeViewModel();
            home.Start(NavigationParameter.Test());
            home.Title = home.Id;
            return home;
        }

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
        
        public RelayCommand AddTabCommand { get; }
    }
}