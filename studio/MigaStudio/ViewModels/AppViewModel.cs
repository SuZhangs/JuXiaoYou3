using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Views;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBased
    {
        public AppViewModel() : base()
        {
            Controller = new TabShell
            {
                WindowStateHandler = x => WindowState = x
            };
            CurrentController = Controller;
        }
    }
}