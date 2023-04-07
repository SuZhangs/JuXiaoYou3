using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(ToolsPage), ViewModel = typeof(ToolsViewModel))]
    public partial class ToolsPage
    {
        public ToolsPage()
        {
            InitializeComponent();
        }
    }
}