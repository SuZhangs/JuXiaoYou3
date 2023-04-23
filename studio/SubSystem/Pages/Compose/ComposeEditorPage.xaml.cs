using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Compose
{

    [Connected(View = typeof(ComposeEditorPage), ViewModel = typeof(ComposeEditorViewModel))]
    public partial class ComposeEditorPage
    {
        public ComposeEditorPage()
        {
            InitializeComponent();
        }
    }
}