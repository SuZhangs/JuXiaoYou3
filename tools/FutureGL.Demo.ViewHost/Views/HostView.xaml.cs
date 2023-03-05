using System.Windows.Controls;
using Acorisoft.FutureGL.Demo.ViewModels;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(HostView), ViewModel = typeof(HostViewModel))]
    public partial class HostView : UserControl
    {
        public HostView()
        {
            InitializeComponent();
        }
    }
}