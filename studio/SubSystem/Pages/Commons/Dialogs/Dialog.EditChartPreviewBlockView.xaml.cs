using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(EditChartPreviewBlockView), ViewModel = typeof(EditChartPreviewBlockViewModel))]
    public partial class EditChartPreviewBlockView
    {
        public EditChartPreviewBlockView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);
        }
    }
}