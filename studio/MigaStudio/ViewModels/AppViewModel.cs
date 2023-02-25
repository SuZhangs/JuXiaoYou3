using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaStudio.Views;

// ReSharper disable ClassNeverInstantiated.Global

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class AppViewModel : TabBased
    {
        public AppViewModel() : base(new TabShell())
        {
            Connect<FirstScreenController, UserControl>();
            Connect<TabShell, TabShellView>();
            CurrentController = Controller;
        }
    }
}