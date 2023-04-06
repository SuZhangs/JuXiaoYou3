using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(EditChartPreviewBlockView), ViewModel = typeof(EditChartPreviewBlockViewModel))]
    public partial class EditChartPreviewBlockView
    {
        public EditChartPreviewBlockView()
        {
            InitializeComponent();
        }
    }
}