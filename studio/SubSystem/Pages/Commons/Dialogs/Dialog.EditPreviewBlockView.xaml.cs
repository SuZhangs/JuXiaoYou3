using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(EditPreviewBlockView), ViewModel = typeof(EditPreviewBlockViewModel))]
    public partial class EditPreviewBlockView
    {
        public EditPreviewBlockView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel<EditPreviewBlockViewModel>().TargetElement = Collection;
            base.OnLoaded(sender, e);
        }
    }
}