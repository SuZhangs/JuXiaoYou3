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

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel<EditPreviewBlockViewModel>().TargetElement = Collection;
            base.OnLoaded(sender, e);
        }
    }
}