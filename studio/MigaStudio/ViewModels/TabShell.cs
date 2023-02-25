using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Lobby;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class TabShell : TabController
    {
        private WindowState _windowState;
        private ITabViewModel _currentViewModel;

        public TabShell()
        {
            Xaml.Get<IWindowEventBroadcast>()
                .PropertyTunnel
                .WindowStateTunnel = x => WindowState = x;
            AddTabCommand = Command(OnAddTabImpl);
        }

        private void OnAddTabImpl()
        {
            if (Onboards.Count < 10)
            {
                Onboards.Add(Test());
            }
            else
            {
                Xaml.Get<INotifyService>().Notify(new IconNotification
                {
                    Title = " 已经添加到。。。" 
                });
                Outboards.Add(Test());
            }
        }
        
        
        private ITabViewModel Test()
        {
            var home = new HomeViewModel();
            home.Start(NavigationParameter.Test());
            home.Title = home.Id;
            return home;
        }

        public ITabViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Suspend();
                
                SetValue(ref _currentViewModel, value);
                
                if (_currentViewModel is null)
                {
                    return;
                }
                
                if (_currentViewModel.Initialized)
                {
                    _currentViewModel.Resume();
                }
                else
                {
                    _currentViewModel.Start();
                }
            }
        }

        /// <summary>
        /// 用于绑定的<see cref="WindowState"/> 属性。
        /// </summary>
        public WindowState WindowState
        {
            get => _windowState;
            set => SetValue(ref _windowState, value);
        }
        
        public RelayCommand AddTabCommand { get; }
    }
}