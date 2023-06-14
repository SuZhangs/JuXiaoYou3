using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(FantasyProjectStartupPage), ViewModel = typeof(FantasyProjectStartupViewModel))]
    public partial class FantasyProjectStartupPage
    {
        public FantasyProjectStartupPage()
        {
            InitializeComponent();
        }
    }
}