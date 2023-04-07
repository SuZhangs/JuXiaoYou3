using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(SimpleDocumentGalleryPage), ViewModel = typeof(SimpleGalleryViewModel))]
    public partial class SimpleDocumentGalleryPage
    {
        public SimpleDocumentGalleryPage()
        {
            InitializeComponent();
        }
    }
}