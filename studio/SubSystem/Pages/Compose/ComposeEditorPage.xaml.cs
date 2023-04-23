using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
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