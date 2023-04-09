using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(ComposeGalleryPage), ViewModel = typeof(ComposeGalleryViewModel))]
    public partial class ComposeGalleryPage
    {
        public ComposeGalleryPage()
        {
            InitializeComponent();
        }
    }
}