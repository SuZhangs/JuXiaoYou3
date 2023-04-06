using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs
{
    [Connected(View = typeof(EditPreviewBlockView), ViewModel = typeof(EditPreviewBlockViewModel))]
    public partial class EditPreviewBlockView
    {
        public EditPreviewBlockView()
        {
            InitializeComponent();
        }
    }
}