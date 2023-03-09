using System.Windows.Controls;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(UserControlHostView), ViewModel = typeof(UserControlHostViewModel))]
    public partial class UserControlHostView : UserControl
    {
        public UserControlHostView()
        {
            InitializeComponent();
        }
    }
}