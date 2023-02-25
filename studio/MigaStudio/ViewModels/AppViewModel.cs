using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Lobby;
using Acorisoft.FutureGL.MigaStudio.Views;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBased
    {
        public AppViewModel() : base()
        {
            var shell = new TabShell
            {
                WindowStateHandler = x => WindowState = x
            };


            var home = new HomeViewModel();
            home.Start(NavigationParameter.Test());
            shell.Onboards.Add(home);
            Controller = shell;
            CurrentController = Controller;
        }
    }
}