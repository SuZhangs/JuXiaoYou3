using System.Threading.Tasks;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Views;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBased
    {
        public AppViewModel() : base()
        {
            var shell = new TabShell();
            Controller        = shell;
            CurrentController = Controller;
        }

        public override Task Start()
        {
            (Controller as TabShell)?.New<HomeViewModel>();
            return base.Start();
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