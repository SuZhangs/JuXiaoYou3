using System.Threading.Tasks;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages;
using Acorisoft.FutureGL.MigaStudio.Views;
using CommunityToolkit.Mvvm.Input;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBaseAppViewModel
    {
        public AppViewModel() : base()
        {
            var shell = new TabShell();
            Controller        = shell;
            CurrentController = new LaunchViewController(this);
        }

        protected override void StartOverride()
        {
            CurrentController.Start();
        }
    }
}