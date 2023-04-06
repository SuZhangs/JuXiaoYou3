using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(NewPreviewBlockView), ViewModel = typeof(NewPreviewBlockViewModel))]
    public partial class NewPreviewBlockView
    {
        public NewPreviewBlockView()
        {
            InitializeComponent();
        }
    }
}