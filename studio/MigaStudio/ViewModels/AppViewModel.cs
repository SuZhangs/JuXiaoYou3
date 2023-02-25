using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Lobby;
using Acorisoft.FutureGL.MigaStudio.Views;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBased
    {
        public AppViewModel() : base()
        {
            var shell = new TabShell
            {
            };
            
            shell.Onboards.Add(Test());
            shell.Onboards.Add(Test());
            shell.Onboards.Add(Test());
            
            Controller        = shell;
            CurrentController = Controller;
        }

        private ITabViewModel Test()
        {
            var home = new HomeViewModel();
            home.Start(NavigationParameter.Test());
            home.Title = home.Id;
            return home;
        }
    }
}