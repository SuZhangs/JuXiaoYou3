using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(EditStringPreviewBlockView), ViewModel = typeof(EditStringPreviewBlockViewModel))]
    public partial class EditStringPreviewBlockView
    {
        public EditStringPreviewBlockView()
        {
            InitializeComponent();
        }

    }
}