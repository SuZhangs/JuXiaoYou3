using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Views
{
    [Connected(View = typeof(LaunchView), ViewModel = typeof(LaunchViewController))]
    public partial class LaunchView : UserControl
    {
        public LaunchView()
        {
            InitializeComponent();
        }
    }
}