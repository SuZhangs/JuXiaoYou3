using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Attributes;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Lobby
{
    [Connected(View = typeof(HomeView), ViewModel = typeof(HomeViewModel))]
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}